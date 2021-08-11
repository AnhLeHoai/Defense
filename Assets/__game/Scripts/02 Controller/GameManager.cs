// #define FORCE_ROLE

using Firebase.Crashlytics;
using RocketTeam.Sdk.Services.Ads;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// Quản lý load scene và game state
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Title("Components")]
    [SerializeField] private UiController uiController;
    [SerializeField] private CameraController mainCamera;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private IapController iap;

    [Space]
    [SerializeField] [BoxGroup("Level Order")] private int[] bonusStepInterstitial = { 5 };
    [SerializeField] [BoxGroup("Level Order")] private int bonusStartIndex = 1;
    [SerializeField] [BoxGroup("Level Order")] private int tutorialStartIndex = 3;
    [SerializeField] [BoxGroup("Level Order")] private int levelStartIndex = 4;

    [Space]
    [Tooltip("Bắt đầu loop từ level bao nhiêu")]
    [SerializeField] [BoxGroup("Loop")] private int loopLevelMin = 5;
    [Tooltip("Level nào không được loop thì add vô đây")]
    [SerializeField] [BoxGroup("Loop")] private List<int> loopExceptionLevels;

    [Title("Prefabs")] 
    [SerializeField] private GameObject fxWinPrefab;
    [SerializeField] public DataTextureSkin dataTextureSkin;
    private GameObject objFxWin;

    private LevelManager currentLevelManager;

    public bool IsLevelLoading { get; private set; }
    public DataLevel DataLevel { get; private set; }
    public int CurrentLevel => DataLevel.Level;
    public GameFSM GameStateController { get; private set; }
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public CameraController MainCamera => mainCamera;
    public UiController UiController => uiController;
    public LevelManager CurrentLevelManager
    {
        get
        {
            if (!currentLevelManager)
            {
                Debug.LogError($"Level {CurrentLevel} does not have a LevelManager or Level has not been loaded!");
                //Crashlytics.Log($"Level {CurrentLevel} does not have a LevelManager or Level has not been loaded!");
            }
            return currentLevelManager;
        }
        private set => currentLevelManager = value;
    }
    public IapController IapController { get => iap; }
    public Profile Profile { get; private set; }
    
    private List<int> loopLevels;

    private void Awake()
    {
        Instance = this;
        GameStateController = new GameFSM(this);
        Profile = new Profile();

        DOTween.Init();
#if UNITY_EDITOR
#else
        Debug.unityLogger.logEnabled = false;
#endif

        DataLevel = PlayerDataManager.GetDataLevel();
    }

    private void Start()
    {
        AdManager.Instance.Init();
        LoadBannerAds();

        LoadLevel();

        UiController.Init();
        //MainCamera.Init();
    }

    /// <summary>
    /// Load level mới và xóa level đang hiện hữu
    /// </summary>
    public void LoadLevel()
    {
        IsLevelLoading = true;
        if (CurrentLevel != 0 && SceneManager.sceneCount != 1)
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));

        // Khi unload scene cũ thì LevelManager cũ cũng bị destroy
        // nhưng cứ gán null cho chắc
        CurrentLevelManager = null;

        int buildIndex = GetCurrentLevelBuildIndex(DataLevel);
        if (buildIndex <= 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("No valid scenes found!");
            GameStateController.ChangeState(GameState.LOBBY);
            return;
        }
        SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        uiController.OpenLoading(true);
    }

    /// <summary>
    /// Trả về scene build index của level hiện tại dựa theo <c>DataLevel</c>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private int GetCurrentLevelBuildIndex(DataLevel dataLevel)
    {
        while (true)
        {
            int buildIndex;
            switch (dataLevel.LevelType)
            {
                case LevelType.Bonus:
                    buildIndex = bonusStartIndex > 0 ? dataLevel.Level + bonusStartIndex - 1 : -1;
                    if (buildIndex < 0)
                    {
                        IncreaseLevel(dataLevel);
                        continue;
                    }

                    break;
                case LevelType.Tutorial:
                    buildIndex = tutorialStartIndex > 0 ? dataLevel.Level + tutorialStartIndex - 1 : -1;
                    if (buildIndex < 0)
                    {
                        IncreaseLevel(dataLevel);
                        continue;
                    }

                    break;
                case LevelType.NormalLevel:
                    buildIndex = dataLevel.Level + levelStartIndex - 1;
                    if (buildIndex <= 0)
                    {
                        if (SceneManager.sceneCountInBuildSettings < 2)
                        {
                            Debug.LogError("There are no level scenes!");
                            buildIndex = -1;
                        }
                        else
                        {
                            buildIndex = 1;
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return buildIndex;
        }
    }

    /// <summary>
    /// Đưa game về state Lobby và khởi tạo lại các giá trị cần thiết cho mỗi level mới.
    /// <remarks>
    /// LevelManager ở mỗi scene khi được load sẽ gọi hàm này.
    /// </remarks>
    /// </summary>
    /// <param name="levelManager"></param>
    public void OnLevelFinishedLoading(LevelManager levelManager)
    {
        CurrentLevelManager = levelManager;
        GameStateController.ChangeState(GameState.LOBBY);
        uiController.OpenLoading(false);
        IsLevelLoading = false;
        uiController.UiMainLobby.Show(true);
    }

    public void StartCurrentLevel()
    {
        Analytics.LogTapToPlay();
        GameStateController.ChangeState(GameState.IN_GAME);
    }

    public void EndCurrentLevel(LevelResult result)
    {
        GameStateController.ChangeState(GameState.END_GAME);

        if (result == LevelResult.Win)
        {
            IncreaseLevel(DataLevel);
        }
    }

    public void DelayedEndgame(LevelResult result)
    {
        StartCoroutine(DelayedEndgameCoroutine(result));
    }

    private IEnumerator DelayedEndgameCoroutine(LevelResult result)
    {
        yield return Yielders.Get(.5f);

        EndCurrentLevel(result);
    }

    private void IncreaseLevel(DataLevel dataLevel)
    {
        switch (dataLevel.LevelType)
        {
            case LevelType.Bonus:
                {
                    // Tìm khoảng cách level để đến level bonus tiếp theo
                    int bonusIndex = dataLevel.BonusStepInterstitialIndex;
                    bonusIndex = ++bonusIndex < bonusStepInterstitial.Length
                        ? bonusIndex
                        : bonusStepInterstitial.Length - 1;
                    dataLevel.BonusStepInterstitialIndex = bonusIndex;
                    dataLevel.LevelCountToBonus = bonusStepInterstitial[bonusIndex];

                    dataLevel.Level = GetNextNormalLevel();
                    dataLevel.LevelType = LevelType.NormalLevel;
                    break;
                }
            case LevelType.Tutorial:
                {
                    if (tutorialStartIndex <= 0 || GetCurrentLevelBuildIndex(dataLevel) + 1 == levelStartIndex)
                    {
                        dataLevel.LevelType = LevelType.NormalLevel;
                        dataLevel.Level = 1;
                        break;
                    }

                    dataLevel.Level++;
                    break;
                }
            case LevelType.NormalLevel:
                {
                    dataLevel.LevelCountToBonus--;
                    if (dataLevel.LevelCountToBonus <= 0)
                    {
                        dataLevel.Level = dataLevel.BonusLevelIndex + 1;
                        dataLevel.LevelType = LevelType.Bonus;

                        dataLevel.BonusLevelIndex = dataLevel.Level;

                        break;
                    }

                    dataLevel.Level = GetNextNormalLevel();
                    break;
                }
            default:
                throw new ArgumentOutOfRangeException();
        }

        dataLevel.DisplayLevel++;
        dataLevel.IsKeyCollected = false;
        PlayerDataManager.SetDataLevel(dataLevel);
    }

    /// <summary>
    /// Trả về level thường tiếp theo dựa vào level tối đa đã đạt được
    /// </summary>
    /// <returns></returns>
    private int GetNextNormalLevel()
    {
        int maxLevelReached = PlayerDataManager.GetMaxLevelReached();
        int maxLevel = SceneManager.sceneCountInBuildSettings - levelStartIndex;
        // Đã đi hết level hay chưa?
        if (maxLevelReached + 1 <= maxLevel)
        {
            return maxLevelReached + 1;
        }

        // Bắt đầu loop
        int nextLevel = loopLevelMin;
        if (loopLevelMin > SceneManager.sceneCountInBuildSettings)
        {
            loopLevelMin = maxLevel;
            if (loopLevelMin == 0)
            {
                return -1;
            }
        }
        
        if (loopLevels == null)
            loopLevels = new List<int>();
        if (loopLevels.Count == 0)
        {
            for (int i = loopLevelMin; i < maxLevel + 1; i++)
            {
                loopLevels.Add(i);
            }
        }

        var index = UnityEngine.Random.Range(0, loopLevels.Count);
        nextLevel = loopLevels[index];
        loopLevels.RemoveAt(index);

        return ++nextLevel > maxLevel ? loopLevelMin : nextLevel;
    }

    public void Revive()
    {
        CurrentLevelManager.Revive();
        GameStateController.ChangeState(GameState.REVIVE);
        SoundManager.Instance.PlaySoundRevive();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        if (GameStateController.currentState.GetId() != (int)GameState.LOBBY
            && GameStateController.currentState.GetId() != (int)GameState.END_GAME)
        {
            UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
        }
    }

    private void Update()
    {
        if (currentLevelManager)
            GameStateController.Update();
    }

    private void FixedUpdate()
    {
        if (currentLevelManager)
            GameStateController.FixedUpdate();
    }

    private void LateUpdate()
    {
        if (currentLevelManager)
            GameStateController.LateUpdate();
    }

    #region Ads
    public void LoadBannerAds()
    {
        if (playerDataManager.IsNoAds())
            return;

        AdManager.Instance.ShowBanner();
    }

    public void ShowInterAds(string _placement)
    {
        AdManager.Instance.ShowInterstitial(_placement, (int)AdEnums.ShowType.INTERSTITIAL);
    }

    public void ShowInterAdsEndGame(string _placement)
    {
        if (playerDataManager.IsNoAds())
            return;

        var config = RocketRemoteConfig.GetStringConfig("config_interstitial_ads", "{\"Level\":1,\"Configs\":[1,1]}");
        var info = JsonUtility.FromJson<InfoInterAds>(config);

        int lvlShowAds = info.Level;

        if (CurrentLevel <= lvlShowAds)
            return;

        int numberPlay = Profile.GetNumberPlay();
        if (numberPlay == -1)
        {
            if (info.Configs[0] == 1)
            {
                Profile.SetNumberPlay(0);
                return;
            }
            else
            {
                numberPlay = 0;
            }
        }

        if (numberPlay >= info.Configs.Length)
            numberPlay = 0;

        switch (info.Configs[numberPlay])
        {
            case 0:
                // ko hien
                break;
            case 1:
                // show ads
                ShowInterAds(_placement);
                break;
            default:
                break;
        }

        numberPlay++;
        Profile.SetNumberPlay(numberPlay);
    }
    #endregion

    public void SpawnEffWin()
    {
        var pos = mainCamera.Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.3f));
        if (objFxWin == null)
        {
            Instantiate(fxWinPrefab, pos, Quaternion.identity, currentLevelManager.transform);
        }
        else
        {
            objFxWin.SetActive(false);
            objFxWin.transform.position = pos;
            objFxWin.SetActive(true);
        }

    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium)]
    [BoxGroup("Level Order")]
    private void UpdateLevelOrder()
    {
        bonusStartIndex = -1;
        tutorialStartIndex = -1;
        levelStartIndex = -1;

        int maxBonusIndex = -1;
        int maxTutorialIndex = -1;
        int maxLevelStartIndex = -1;

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (Regex.IsMatch(sceneName, "Level Bonus \\d"))
            {
                bonusStartIndex = bonusStartIndex == -1 ? i : bonusStartIndex;
                maxBonusIndex = i;
            }
            else if (Regex.IsMatch(sceneName, "Level Tutorial \\d"))
            {
                tutorialStartIndex = tutorialStartIndex == -1 ? i : tutorialStartIndex;
                maxTutorialIndex = i;
            }
            else if (Regex.IsMatch(sceneName, "Level \\d"))
            {
                levelStartIndex = levelStartIndex == -1 ? i : levelStartIndex;
                maxLevelStartIndex = i;
            }
            else
            {
                Debug.LogError($"Scene at build index {i} does not follow Smasher scene's naming convention. Please check again or scenes might be loaded unexpectedly!");
            }
        }

        if (maxBonusIndex > tutorialStartIndex && tutorialStartIndex != -1)
        {
            Debug.LogError("Bonus scene should not have higher index than tutorial scene ");
        }

        if (maxBonusIndex > levelStartIndex && levelStartIndex != -1)
        {
            Debug.LogError("Bonus scene should not have higher index than level scene ");
        }

        if (maxTutorialIndex > levelStartIndex && levelStartIndex != -1)
        {
            Debug.LogError("Tutorial scene should not have higher index than level scene ");
        }
    }
#endif
}
