using DG.Tweening;
using RocketTeam.Sdk.Services.Ads;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UiMainLobby : UICanvas
{
    [Title("Button")]
    [SerializeField] private Button btnHide;
    [SerializeField] private Button btnLuckySpin;
    [SerializeField] private Button btnSkin;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnNoAds;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnGift;

    [Title("Animation")]
    [SerializeField] private GameObject AnimationLeft;
    [SerializeField] private GameObject AnimationRight;

    [Title("Others")]
    [SerializeField] private List<Image> listImgKeys;
    [SerializeField] private Text txtCurentLevel;
    public Button BtnNoAds { get => btnNoAds; }
    public Button BtnPlay => btnHide;
    private bool isFistOpen;

    // Start is called before the first frame update
    void Start()
    {
        btnHide.onClick.AddListener(OnClickBtnPlay);
        btnLuckySpin.onClick.AddListener(OnClickBtnSpin);
        btnSkin.onClick.AddListener(OnClickShopSkin);
        btnShop.onClick.AddListener(OnClickBtnShopIap);
        btnNoAds.onClick.AddListener(OnClickBtnNoAds);
        btnSetting.onClick.AddListener(OnClickBtnSetting);
        btnGift.onClick.AddListener(OnClickBtnGift);
        Init();
    }

    private void Init()
    {
        SetLayoutKey();
        DataLevel dataLevel = GameManager.Instance.PlayerDataManager.GetDataLevel();
        txtCurentLevel.text = string.Format("LEVEL {0}", dataLevel.DisplayLevel);
        //GameManager.Instance.MainCamera.SetupCamera();
        if (GameManager.Instance.PlayerDataManager.IsNoAds())
        {
            btnNoAds.interactable = false;
        }


    }


    public void SetLayoutKey()
    {
        for (int i = 0; i < listImgKeys.Count; i++)
        {
            listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(false);
        }

        int countKey = GameManager.Instance.Profile.GetKey() > 3 ? 3 : GameManager.Instance.Profile.GetKey();
        for (int i = 0; i < countKey; i++)
        {
            listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(true);
        }
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (IsShow)
        {
            Init();
            btnGift.gameObject.SetActive(IsShowGift());
        }
        else
        {
            BtnPlay.gameObject.SetActive(false);
        }

    }


    private void OnClickBtnPlay()
    {
        if (GameManager.Instance.CurrentLevelManager == null)
            return;

        GameManager.Instance.StartCurrentLevel();
        ShowAniHide();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnImposter()
    {
        GameManager.Instance.StartCurrentLevel();
        ShowAniHide();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnSpin()
    {
        GameManager.Instance.UiController.OpenLuckyWheel();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickShopSkin()
    {
        GameManager.Instance.UiController.OpenShopCharacter();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnShopIap()
    {
        GameManager.Instance.UiController.OpenShopIap();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnNoAds()
    {
        GameManager.Instance.IapController.PurchaseProduct((int)IdPack.NO_ADS_BASIC);
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnSetting()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public void ShowAniHide()
    {
        Show(false);
    }

    public void ActiveMainLobby()
    {
        Show(true);
    }

    public void Hack()
    {
        GameManager.Instance.Profile.AddGold(100000, "");
    }

    private void OnClickBtnGift()
    {
#if UNITY_EDITOR
        OnRewardGift(1);


#else
        //AdManager.Instance.ShowAdsReward(OnRewardVideo, Helper.video_reward_lucky_wheel);
        WaitingCanvas.Instance.Show("");
        Observable.FromCoroutine(ActionWatchVideo).Subscribe().AddTo(this.gameObject);
#endif
    }

    private IEnumerator ActionWatchVideo()
    {
        float _tmp1 = 0;
        float _tmp2 = 1;
        RewardAdStatus _rewardAdStatus = RewardAdStatus.NoVideoNoInterstitialReward;
        while (_tmp1 < 2)
        {
            _tmp1 += Time.deltaTime;
            _tmp2 += Time.deltaTime;
            if (_tmp2 > 0.5f)
            {
                _tmp2 = 0;
                _rewardAdStatus = AdManager.Instance.ShowAdsReward(OnRewardGift, Helper.video_reward_gift_box, false);
                switch (_rewardAdStatus)
                {
                    case RewardAdStatus.NoInternet:
                        WaitingCanvas.Instance.Hide();
                        PopupDialogCanvas.Instance.Show("No Internet!");

                        Analytics.LogEventByName("Monetize_reward_no_internet");
                        Analytics.LogEventByName("Monetize_interstitial_no_internet");
                        yield break;
                    case RewardAdStatus.NoVideoNoInterstitialReward:
                        break;
                    default:
                        WaitingCanvas.Instance.Hide();
                        yield break;
                }
            }
            yield return null;
        }
        if (_rewardAdStatus == RewardAdStatus.NoVideoNoInterstitialReward)
        {
            WaitingCanvas.Instance.Hide();
            PopupDialogCanvas.Instance.Show("No Video!");
            Analytics.LogEventByName("Monetize_no_reward");
            Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
        }
    }

    private void OnRewardGift(int x)
    {
        GameManager.Instance.PlayerDataManager.SetTimeLoginOpenGift(DateTime.Now.ToString());
        btnGift.gameObject.SetActive(false);
        if (isFistOpen)
        {
            isFistOpen = false;
            int idSkin = GetIdSkin();
            int idHat = GetIdHat();
            if (idSkin != -1)
            {
                RewardEndGame reward = new RewardEndGame();
                reward.Type = TypeEquipment.SKIN;
                reward.Id = idSkin;

                GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                GameManager.Instance.PlayerDataManager.SetUnlockSkin(TypeEquipment.SKIN, idSkin);
            }
            else if (idHat != -1)
            {
                RewardEndGame reward = new RewardEndGame();
                reward.Type = TypeEquipment.HAT;
                reward.Id = idHat;

                GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                GameManager.Instance.PlayerDataManager.SetUnlockSkin(TypeEquipment.HAT, reward.Id);
            }
            else
            {
                int gold = UnityEngine.Random.Range(200, 1000);
                GameManager.Instance.Profile.AddGold(gold, "gift_box");

                SoundManager.Instance.PlaySoundReward();
            }
            

        }
        else
        {
            var rd = UnityEngine.Random.Range(0, 100);
            if (rd < 80)
            {
                int gold = UnityEngine.Random.Range(200, 1000);
                GameManager.Instance.Profile.AddGold(gold, "gift_box");

                SoundManager.Instance.PlaySoundReward();
            }
            else
            {
                int idSkin = GetIdSkin();
                int idHat = GetIdHat();
                if (idSkin != -1)
                {
                    RewardEndGame reward = new RewardEndGame();
                    reward.Type = TypeEquipment.SKIN;
                    reward.Id = idSkin;

                    GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                    GameManager.Instance.PlayerDataManager.SetUnlockSkin(TypeEquipment.SKIN, reward.Id);
                }
                else if (idHat != -1)
                {
                    RewardEndGame reward = new RewardEndGame();
                    reward.Type = TypeEquipment.HAT;
                    reward.Id = idHat;

                    GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                    GameManager.Instance.PlayerDataManager.SetUnlockSkin(TypeEquipment.HAT, idHat);
                }
                else
                {
                    int gold = UnityEngine.Random.Range(200, 1000);
                    GameManager.Instance.Profile.AddGold(gold, "gift_box");

                    SoundManager.Instance.PlaySoundReward();
                }
            }
        }

    }

    private int GetIdSkin()
    {
        var data = GameManager.Instance.UiController.ShopCharater.dataShopSkin.listDataShopSkins;
        List<int> listId = new List<int>();
        for (int i = 0; i < data.Count; i++)
        {
            if (!GameManager.Instance.PlayerDataManager.GetUnlockSkin(TypeEquipment.SKIN, data[i].idSkin) && !data[i].typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
            {
                listId.Add(data[i].idSkin);
            }
        }

        if (listId.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, listId.Count);
            return listId[index];


        }
        else
        {
            return -1;
        }
    }

    private int GetIdHat()
    {
        var data = GameManager.Instance.UiController.ShopCharater.dataShopSkin.listDataShopHats;
        List<int> listId = new List<int>();
        for (int i = 0; i < data.Count; i++)
        {
            if (!GameManager.Instance.PlayerDataManager.GetUnlockSkin(TypeEquipment.HAT, data[i].idSkin) && !data[i].typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
            {
                listId.Add(data[i].idSkin);
            }
        }

        if (listId.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, listId.Count);
            return listId[index];


        }
        else
        {
            return -1;
        }
    }

    private bool IsShowGift()
    {

        string time = GameManager.Instance.PlayerDataManager.GetTimeLoginOpenGift();
        if (string.IsNullOrEmpty(time))
        {
            isFistOpen = true;

            return true;
        }


        DateTime timeLogin = DateTime.Parse(time);

        long tickTimeNow = DateTime.Now.Ticks;
        long tickTimeOld = timeLogin.Ticks;

        long elapsedTicks = tickTimeNow - tickTimeOld;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

        float totalSeconds = (float)elapsedSpan.TotalSeconds;

        var totalTimeReset = 600 - totalSeconds;
        if (totalTimeReset <= 0)
        {
            GameManager.Instance.PlayerDataManager.SetTimeLoginOpenGift(DateTime.Now.ToString());
            return true;
        }
        else
        {
            return false;
        }

    }
}
