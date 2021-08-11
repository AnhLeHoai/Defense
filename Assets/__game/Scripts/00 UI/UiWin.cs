using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiWin : UICanvas
{
    [SerializeField] private Button btnCollectGold;
    [SerializeField] private Button btnX3Coin;
    [SerializeField] private Image iconRewardBg;
    [SerializeField] private Image iconReward;
    [SerializeField] private Image imgProcessReward;
    [SerializeField] private Text txtProcessReward;
    [SerializeField] private Text txtCoin;
    [SerializeField] private Text txtCoinBonus;
    [SerializeField] private Text txtCoinReward;
    [SerializeField] private DataRewardEndGame dataRewards;
    [SerializeField] private GameObject objFx;
    private int percentReward;
    private int _gold;
    private int ratioBonus;
    private void Start()
    {
        btnCollectGold.onClick.AddListener(OnClickBtnNoThank);
        btnX3Coin.onClick.AddListener(OnClickBtnX3Coin);
    }

    public void Init(int gold)
    {
        _gold = gold;
        ratioBonus = 3;
        objFx.SetActive(false);

        txtCoin.text = string.Format("COLLECT {0}", gold);
        txtCoinBonus.text = string.Format("+{0}", gold * ratioBonus);
        btnX3Coin.gameObject.SetActive(true);
        btnCollectGold.interactable = true;
        SetupRewardEndGame();

        StartCoroutine(IEWaitShowFx());
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (!isShow)
        {
            //GameManager.Instance.UiController.MoreGame.Show(false);
            GameManager.Instance.ShowInterAdsEndGame("end_game_win");
        }

    }

    private void SetupRewardEndGame()
    {
        txtCoinReward.gameObject.SetActive(false);
        var playerData = GameManager.Instance.PlayerDataManager;
        int indexReward = playerData.GetCurrentIndexRewardEndGame();
        if (indexReward >= dataRewards.Datas.Count)
        {
            indexReward = 0;
            playerData.SetCurrentIndexRewardEndGame(indexReward);
        }

        var reward = dataRewards.Datas[indexReward];
        if (playerData.GetUnlockSkin(reward.Type, reward.Id))
        {
            iconReward.sprite = playerData.DataTexture.IconCoin;
            txtCoinReward.text = string.Format("+{0}", reward.NumberCoinReplace);
            txtCoinReward.gameObject.SetActive(true);
        }
        else
        {
            switch (reward.Type)
            {
                case TypeEquipment.HAT:
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconHat(reward.Id);
                    }

                    break;
                case TypeEquipment.SKIN:
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconSkin(reward.Id);
                    }
                    break;
                case TypeEquipment.PET:
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconPet(reward.Id);
                    }
                    break;
                case TypeEquipment.SKILL:
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconSkill(reward.Id);
                    }
                    break;
            }



        }

        iconRewardBg.sprite = iconReward.sprite;
        iconRewardBg.SetNativeSize();
        iconReward.SetNativeSize();

        int process = playerData.GetProcessReceiveRewardEndGame();
        process++;
        playerData.SetProcessReceiveRewardEndGame(process);
        if (process >= reward.NumberWin)
        {
            playerData.SetProcessReceiveRewardEndGame(0);

            StartCoroutine(IEWaitShowRewardEndGame(reward));

        }
        else
        {
            // check show rate game
            int lvlShowRate = RocketRemoteConfig.GetIntConfig("config_show_rate_game", 1000);
            if (PlayerPrefs.GetInt("showRate", 0) == 0 && GameManager.Instance.PlayerDataManager.GetDataLevel().DisplayLevel >= lvlShowRate)
            {
                StartCoroutine(IEShowRateGame());
                PlayerPrefs.SetInt("showRate", 1);
            }
        }

        float ratio = (float)process / (float)reward.NumberWin;
        imgProcessReward.fillAmount = 0;
        imgProcessReward.DOFillAmount(ratio, 1f);
        percentReward = (int)(ratio * 100);

        iconReward.fillAmount = 0;
        iconReward.DOFillAmount(ratio, 1f);

        SetPercentReward(percentReward);
    }

    private Tweener tweenCoin;
    private int tmpPercent;
    private void SetPercentReward(int percent)
    {
        tweenCoin = tweenCoin ?? DOTween.To(() => tmpPercent, x =>
        {
            tmpPercent = x;
            txtProcessReward.text = string.Format("{0}%", tmpPercent);
        }, percent, 1f).SetAutoKill(false).OnComplete(() =>
         {
             tmpPercent = percentReward;
             txtProcessReward.text = string.Format("{0}%", tmpPercent);
         });

        tweenCoin.ChangeStartValue(tmpPercent);
        tweenCoin.ChangeEndValue(percent);
        tweenCoin.Play();
    }

    private void OnClickBtnNoThank()
    {
        //OnBackPressed();
        //GameManager.Instance.LoadLevel();
        GameManager.Instance.Profile.AddGold(_gold, "end_game");
        btnCollectGold.interactable = false;
        SoundManager.Instance.PlaySoundReward();
        StartCoroutine(IEGoLobby());
    }

    private void OnClickBtnX3Coin()
    {
#if UNITY_EDITOR
        OnRewardVideo(1);
#else
        RewardAdStatus adStatus = AdManager.Instance.ShowAdsReward((x) =>
        {
            OnRewardVideo(1);

        }, Helper.video_reward_x3_gold_end_game, true);

        switch (adStatus)
        {
            case RewardAdStatus.NoInternet:
                PopupDialogCanvas.Instance.Show("No Internet!");
                break;
            case RewardAdStatus.ShowVideoReward:
                break;
            case RewardAdStatus.ShowInterstitialReward:
                break;
            case RewardAdStatus.NoVideoNoInterstitialReward:
                PopupDialogCanvas.Instance.Show("No Video!");
                break;
            default:
                break;
        }
#endif
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnRewardVideo(int x)
    {
        if (x <= 0 && !isShow)
            return;

        btnX3Coin.gameObject.SetActive(false);
        btnCollectGold.interactable = false;
        GameManager.Instance.Profile.AddGold(_gold * ratioBonus, Helper.video_reward_end_game);

        //txtCoin.text = string.Format("+{0}", _gold * 3);
        SoundManager.Instance.PlaySoundReward();

        StartCoroutine(IEGoLobby());
    }

    private IEnumerator IEWaitShowRewardEndGame(RewardEndGame reward)
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.END_GAME);
    }

    private IEnumerator IEWaitShowFx()
    {
        yield return new WaitForSeconds(0.5f);
        objFx.SetActive(true);
    }

    private IEnumerator IEShowRateGame()
    {
        yield return new WaitForSeconds(0.5f);
        PopupRateGame.Instance.Show();
    }

    private IEnumerator IEGoLobby()
    {
        yield return new WaitForSeconds(0.5f);
        OnBackPressed();
        GameManager.Instance.LoadLevel();
    }
}
