using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketConfig
{
    public const string Appsflyer_Devkey = "Z2ty7rjatUSWqcybH48vGU";
    public const string package_name = "com.casual.roblox.smasher";

    public static string default_cp = "default_bigfox";
    public const string VersionName = "1.0.4";
    public const int versionCode = 20005;
    public const string ProductName = "Roblock Smashers";
    public const string settingKeyStore = "roblox_smasher";
    public const string settingLogo = "GAME_ICON";

    public const string settingAliasName = "com.casual.roblox.smasher";

    public const string keyaliasPass = "com.casual.roblox.smasher";
    public const string keystorePass = "com.casual.roblox.smasher";

    public const string MORE_GAME_URI = "http://lovemoney.vn:8080/FatCAt/apps/search/findByType";



#if UNITY_ANDROID
    public const string OPEN_LINK_RATE = "market://details?id=" + package_name;
#else 
    public static string OPEN_LINK_RATE = "itms-apps://itunes.apple.com/app/id"+Apple_App_ID;
#endif

    public const string Apple_App_ID = "1530916159";
    public static string inappAndroidKeyHash = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt2M0VamyC0bsFIN9KX29ts1P6Ab8LVcToyBbBxHs3rIu1eSUrlj8GSn2Bl4jWuMgDgDNTvk4KO9XuIqnPJSrWBOoi65LT86lirncrGSg6aiAp/jr/Wo35SgvXvOnJhtIDIBbduQGnNGIJTl8lukO8NzeL/TVAEvRPrUym8joGdWiQ/kgjFS2aAUG8V9cs3IAt9hzPwCCCMVuGcU9ZMe0rC/kOtWVFgg3DsRIOkNvkZEXzsNw0ZJZ9g2lqon9VXPzxZh7Bj085n7qF1SrXeQ3BGnQcGuDF/gephGLI+35G8/OkVH0d05GbDI6EgKQK1sL9rDPOJcq3qOJ5Bw+GLKA7wIDAQAB";
}
