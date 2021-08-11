#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

//[InitializeOnLoadAttribute]
public class ConfigBuild : EditorWindow
{
    static bool flagchange;
    private static string KeyStorePath = "_SDK/KeyStore/{0}.keystore";
    private static string ActiveKeyStorenPath = "_SDK/KeyStore/Active/ActiveKeyStore.keystore";

    [MenuItem("JOKER2X/Open File Build &_o")]
    public static void OpenFileBuild()
    {
        string path = Path.Combine(Application.dataPath, string.Format("../Builds"));
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = path;
        proc.Start();
    }
    
    public static void ConvertAabToApks(string path)
    {
        path = Path.ChangeExtension(path, "aab");
        Debug.LogError(path);
        if (!File.Exists(path)) return;
        string filename = Path.GetFileNameWithoutExtension(path);
        string keystore = Path.Combine(Application.dataPath, String.Format(KeyStorePath, RocketConfig.settingKeyStore)).Replace("/", "\\");
        string keystorePass = RocketConfig.keystorePass;
        string keystoreAlias = RocketConfig.settingAliasName;
        string keystoreAliasPass = RocketConfig.keyaliasPass;

        #region commands
        
        // Get bundletool.jar from UnityEditor
        string bundletoolPath = Path.Combine(EditorApplication.applicationPath,
            "../Data/PlaybackEngines/AndroidPlayer/Tools");
        bundletoolPath = Directory.GetFiles(bundletoolPath, "bundletool*.jar", SearchOption.TopDirectoryOnly).FirstOrDefault();
        
        // Running java command to execute bundletool and build apks file
        string buildApksCmd =
            $" java -jar \"{bundletoolPath}\" build-apks "
            + $"--bundle=\"{filename}.aab\" --output=\"{filename}.apks\" --mode=universal "
            + $"--ks=\"{keystore}\" --ks-pass=pass:{keystorePass} "
            + $"--ks-key-alias=\"{keystoreAlias}\" --key-pass=pass:{keystoreAliasPass} ";
        
        // Apks to Zip
        string renameToZipCmd = $" rename \"{filename}.apks\"  \"{filename}\".zip";
        
        // Wait for command to finish execute
        string pauseCmd = $" PAUSE ";

        #endregion

        var process = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                WorkingDirectory = Path.GetDirectoryName(path) ?? string.Empty,
                RedirectStandardInput = true,
                UseShellExecute = false
            }
        };
        
        process.Start();
        using (StreamWriter sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                sw.WriteLine(buildApksCmd);
                sw.WriteLine(renameToZipCmd);
                sw.WriteLine(pauseCmd);
            }
        }
    }

    [MenuItem("JOKER2X/Open Scenes/MasterLevel &_1")]
    public static void OpenSceneSplash()
    {
        string scenePath = "Assets/__game/Scenes/MasterLevel.unity";
        EditorSceneManager.OpenScene(scenePath);
    }
    
    [MenuItem("JOKER2X/Open Scenes/TestScene &_2")]
    public static void OpenSceneLevelTest()
    {
        string scenePath = "Assets/__game/Scenes/TestScene.unity";
        EditorSceneManager.OpenScene(scenePath);
    }
    
    [MenuItem("JOKER2X/Build Game &_b")]
    public static void BuildGame()
    {
        Build();
        string[] levels = EditorBuildSettings.scenes.Where(x => x.enabled).Select(scene => scene.path).ToArray();
        var report = BuildPipeline.BuildPlayer(levels, GetFilePath("All"), BuildTarget.Android, BuildOptions.None);
        ConvertAabToApks(report.summary.outputPath);
        OpenFileBuild();
    }

    public static void Build()
    {
        flagchange = false;
#if UNITY_IOS || UNITY_ANDROID
        if (RocketConfig.package_name.CompareTo(PlayerSettings.applicationIdentifier) != 0)
        {
            PlayerSettings.applicationIdentifier = RocketConfig.package_name;
            flagchange = true;
        }
        if (RocketConfig.VersionName.CompareTo(PlayerSettings.bundleVersion) != 0)
        {
            PlayerSettings.bundleVersion = RocketConfig.VersionName;
            flagchange = true;
        }
#endif
#if UNITY_ANDROID
        PlayerSettings.SetScriptingBackend(EditorUserBuildSettings.selectedBuildTargetGroup, ScriptingImplementation.IL2CPP);
        if (RocketConfig.versionCode.CompareTo(PlayerSettings.Android.bundleVersionCode) != 0)
        {
            PlayerSettings.Android.bundleVersionCode = RocketConfig.versionCode;
            flagchange = true;
        }
        if (RocketConfig.ProductName.CompareTo(PlayerSettings.productName) != 0)
        {
            PlayerSettings.productName = RocketConfig.ProductName;
            flagchange = true;
        }
        PlayerSettings.Android.keyaliasName = RocketConfig.settingAliasName;
        PlayerSettings.Android.keyaliasPass = RocketConfig.keyaliasPass;
        PlayerSettings.Android.keystorePass = RocketConfig.keystorePass;
#endif
        //update keystore
        SwapAsset(Path.Combine(Application.dataPath, String.Format(KeyStorePath, RocketConfig.settingKeyStore)), Path.Combine(Application.dataPath, ActiveKeyStorenPath));
        AssetDatabase.Refresh();

        if (true)
        {
            Debug.LogFormat("Config loaded|package name:=<color={0}>{1}</color>|CP:=<color={0}>{2}</color>|Version Name:=<color={0}>{3}</color>|Version Code:=<color={0}>{4}</color>|ProductName=<color={0}>{5}</color>"
                            , "#FF33CC"
                            , PlayerSettings.applicationIdentifier
                            , RocketConfig.default_cp, PlayerSettings.bundleVersion
                            , PlayerSettings.Android.bundleVersionCode
                            , PlayerSettings.productName);
        }
    }

    static void GenerateCP()
    {
        var outputFile = Path.Combine(Application.dataPath, "Plugins/Android/assets/raw/cp");
        if (File.Exists(outputFile))
        {
            string cp = File.ReadAllText(outputFile);
            if (cp.CompareTo(RocketConfig.default_cp) != 0)
            {
                File.WriteAllText(outputFile, RocketConfig.default_cp);
                flagchange = true;
            }
        }
    }
    public static void UpdateManifest()
    {
        var fullPath = Path.Combine(Application.dataPath, "Plugins/Android/OneSignalConfig/AndroidManifest.xml");
        string UnityGCMReceiver = "com.onesignal.GcmBroadcastReceiver";
        string remaneC2DM = ".permission.C2D_MESSAGE";
        XmlDocument doc = new XmlDocument();
        doc.Load(fullPath);
        if (doc == null)
        {
            Debug.LogError("Couldn't load " + fullPath);
            return;
        }

        XmlNode manNode = FindChildNode(doc, "manifest");
        XmlNode dict = FindChildNode(manNode, "application");

        if (dict == null)
        {
            Debug.LogError("Error parsing " + fullPath);
            return;
        }
        string ns = dict.GetNamespaceOfPrefix("android");
        XmlElement renameGCM = FindElementWithAndroidName("receiver", "name", ns, UnityGCMReceiver, dict);
        XmlNode curr = renameGCM.FirstChild;
        XmlElement element = (XmlElement)FindChildNode(curr, "category");
        if (element.Name.CompareTo(RocketConfig.package_name) == 0) return;
        element.SetAttribute("name", ns, RocketConfig.package_name);
        curr = manNode.FirstChild;
        while (curr != null)
        {
            try
            {
                element = (XmlElement)curr;
            }
            catch (Exception e)
            {
            }
            if (element.GetAttribute("name", ns).Contains("C2D_MESSAGE"))
            {
                element.SetAttribute("name", ns, RocketConfig.package_name + remaneC2DM);
            }
            curr = curr.NextSibling;
        }
        XmlDocument docompare = new XmlDocument();
        docompare.Load(fullPath);
        if (doc.InnerXml.CompareTo(docompare.InnerXml) != 0)
        {
            doc.Save(fullPath);
            flagchange = true;
        }
    }
    private static XmlNode FindChildNode(XmlNode parent, string name)
    {
        XmlNode curr = parent.FirstChild;
        while (curr != null)
        {
            if (curr.Name.Equals(name))
            {
                return curr;
            }
            curr = curr.NextSibling;
        }
        return null;
    }
    private static XmlElement FindElementWithAndroidName(string name, string androidName, string ns, string value, XmlNode parent)
    {
        var curr = parent.FirstChild;
        while (curr != null)
        {
            if (curr.Name.Equals(name) && curr is XmlElement && ((XmlElement)curr).GetAttribute(androidName, ns) == value)
            {
                return curr as XmlElement;
            }
            curr = curr.NextSibling;
        }
        return null;
    }
    private static void SwapAsset(string source, string target)
    {
        Debug.Log("Using: " + source);
        if (System.IO.File.Exists(target))
        {
            Debug.Log("Updating: " + target);
            FileUtil.ReplaceFile(source, target);
        }
        else
        {
            Debug.Log("Creating: " + target);
            FileUtil.CopyFileOrDirectory(source, target);
        }
    }

    static string GetFileName(string surFix)
    {
        string outputName = GetArg("-outputName");
        return string.Format("{0}{1}", outputName ?? RocketConfig.ProductName, surFix);
    }

    static string GetFilePath(string surFix)
    {
        string outputPath = GetArg("-outputPath");
        string gameName = GetValidFileName(RocketConfig.ProductName);
        if (outputPath != null)
        {
            return $"{outputPath}/{gameName}_{surFix}_{DateTime.Now.ToString("ddMMyyyy_h-mm-tt")}.apk";
        }
        else
        {
            return Path.Combine(Application.dataPath,
                $"../Builds/{gameName}_{surFix}_{DateTime.Now.ToString("ddMMyyyy_h-mm-tt")}.apk");
        }
    }

    static string GetValidFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }

        return fileName;
    }
    
    static string GetFilePathReplace(string surFix)
    {
        string outputPath = GetArg("-outputPath");
        if (outputPath != null)
        {
            return string.Format("{0}/{1}.apk", outputPath, "Smasher_" + surFix);
        }
        else
        {
            return Path.Combine(Application.dataPath, string.Format("../Builds/{0}.apk", "Smasher_" + surFix));
        }
    }

    static string GetArg(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
#endif