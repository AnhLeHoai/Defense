using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupRewardEndGame : UICanvas
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private Button btnVideo;
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnClaim;

    [SerializeField] private Text txtCoinReplace;
    private RewardEndGame _reward;
    [SerializeField] private GameObject objParentRewards;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClickBtnClose);
        btnVideo.onClick.AddListener(OnClickBtnVideo);
        btnClaim.onClick.AddListener(OnClickBtnClose);
    }

    public void Init(RewardEndGame reward, TypeDialogReward type)
    {

        _reward = reward;
        objParentRewards.SetActive(false);
        txtCoinReplace.gameObject.SetActive(false);
        var playerData = GameManager.Instance.PlayerDataManager;
        if (playerData.GetUnlockSkin(reward.Type, reward.Id))
        {
            imgIcon.sprite = playerData.DataTexture.IconCoin;
            txtCoinReplace.text = string.Format("+{0}", reward.NumberCoinReplace);
            txtCoinReplace.gameObject.SetActive(true);
        }
        else
        {
            switch (reward.Type)
            {
                case TypeEquipment.HAT:
                    {
                        imgIcon.sprite = playerData.DataTexture.GetIconHat(reward.Id);

                    }

                    break;
                case TypeEquipment.SKIN:
                    {
                        imgIcon.sprite = playerData.DataTexture.GetIconSkin(reward.Id);
                    }
                    break;
                case TypeEquipment.PET:
                    {
                        imgIcon.sprite = playerData.DataTexture.GetIconPet(reward.Id);
                    }
                    break;
                case TypeEquipment.SKILL:
                    {
                        imgIcon.sprite = playerData.DataTexture.GetIconSkill(reward.Id);
                    }
                    break;
            }



        }
        imgIcon.SetNativeSize();

        switch (type)
        {
            case TypeDialogReward.LUCKY_WHEEL:
                {
                    btnClaim.gameObject.SetActive(true);
                    btnVideo.gameObject.SetActive(false);
                    btnClose.gameObject.SetActive(false);

                    txtCoinReplace.gameObject.SetActive(false);
                }
                break;
            case TypeDialogReward.END_GAME:
                {
                    btnClaim.gameObject.SetActive(false);
                    btnVideo.gameObject.SetActive(true);
                    btnClose.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    private void OnClickBtnVideo()
    {
#if UNITY_EDITOR
        OnRewardedVideo(1);
#else
        RewardAdStatus adStatus = AdManager.Instance.ShowAdsReward((x) =>
        {
            OnRewardedVideo(1);

        }, Helper.video_reward_end_game, true);

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

    private void OnClickBtnClose()
    {
        bool isNextReward = RocketRemoteConfig.GetBoolConfig("next_reward_end_game_user_lose_it", true);
        if (isNextReward)
        {
            SetupNextReward();
        }
        OnBackPressed();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnRewardedVideo(int x)
    {
        if (x <= 0 && !isShow)
            return;

        if (GameManager.Instance.PlayerDataManager.GetUnlockSkin(_reward.Type, _reward.Id))
        {
            GameManager.Instance.Profile.AddGold(_reward.NumberCoinReplace, Helper.video_reward_end_game);
        }
        else
        {
            GameManager.Instance.PlayerDataManager.SetUnlockSkin(_reward.Type, _reward.Id);
            GameManager.Instance.PlayerDataManager.SetIdEquipSkin(_reward.Type, _reward.Id);
        }

        SetupNextReward();
        OnBackPressed();
    }

    public void ActiveReward()
    {
        objParentRewards.SetActive(true);
        SoundManager.Instance.PlaySoundReward();

    }

    private void SetupNextReward()
    {
        var indexReward = GameManager.Instance.PlayerDataManager.GetCurrentIndexRewardEndGame();
        GameManager.Instance.PlayerDataManager.SetProcessReceiveRewardEndGame(0);
        indexReward++;
        GameManager.Instance.PlayerDataManager.SetCurrentIndexRewardEndGame(indexReward);
    }
}
