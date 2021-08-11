using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemHat : ItemSkinShop
{

    public override void Init(DataShop data)
    {
        base.Init(data);
        var playerData = GameManager.Instance.PlayerDataManager;
        icon.sprite = playerData.DataTexture.GetIconHat(data.idSkin);
        icon.SetNativeSize();
        ImgBg.sprite = GameManager.Instance.PlayerDataManager.DataTexture.ListSprBgItemShops[0]; // not selected
        if (playerData.GetUnlockSkin(TypeEquipment.HAT, data.idSkin))
        {
            if (playerData.GetIdEquipSkin(TypeEquipment.HAT) == data.idSkin)
            {
                listBtnBehavior[(int)TypeButtonBehavior.REMOVE].gameObject.SetActive(true);
                ImgBg.sprite = GameManager.Instance.PlayerDataManager.DataTexture.ListSprBgItemShops[1];
            }
            else
            {
                listBtnBehavior[(int)TypeButtonBehavior.USE].gameObject.SetActive(true);
            }
        }
        else
        {
            if (data.typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
            {
                listBtnBehavior[(int)TypeButtonBehavior.SPIN].gameObject.SetActive(true);
            }

            if (data.typeUnlock.HasFlag(TypeUnlockSkin.COIN))
            {
                listBtnBehavior[(int)TypeButtonBehavior.UNLOCK_BY_COIN].gameObject.SetActive(true);
            }

            if (data.typeUnlock.HasFlag(TypeUnlockSkin.VIDEO))
            {
                listBtnBehavior[(int)TypeButtonBehavior.UNLOCK_BY_VIDEO].gameObject.SetActive(true);
                int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.HAT, data.idSkin);

                txtVideo.text = string.Format("{0}/{1}", numberWatchVideo, data.numberVideoUnlock);
            }
        }
    }

    protected override void OnClickBtnView()
    {
        base.OnClickBtnView();
        shopCharacter.SkinCharacterController.ChangeSkin(TypeEquipment.HAT, dataShop.idSkin);
    }

    protected override void OnClickBtnBehaviour(int idBehaviour)
    {
        base.OnClickBtnBehaviour(idBehaviour);
        var playerData = GameManager.Instance.PlayerDataManager;

        switch ((TypeButtonBehavior)idBehaviour)
        {
            case TypeButtonBehavior.SPIN:
                {
                    shopCharacter.OnBackPressed();
                    GameManager.Instance.UiController.OpenLuckyWheel();
                }
                break;
            case TypeButtonBehavior.UNLOCK_BY_VIDEO:
                {
                    //AdManager.Instance.ShowAdsReward(OnRewardedVideo, Helper.video_shop_hat);
                    WaitingCanvas.Instance.Show("");
                    Observable.FromCoroutine(ActionWatchVideo).Subscribe().AddTo(this.gameObject);
                }
                break;
            case TypeButtonBehavior.UNLOCK_BY_COIN:
                {
                    if (GameManager.Instance.Profile.GetGold() < dataShop.numberCoinUnlock)
                    {
                        shopCharacter.ActiveNotiNotEnoughtGold(this.transform);

                        return;
                    }

                    playerData.SetUnlockSkin(TypeEquipment.HAT, dataShop.idSkin);
                    GameManager.Instance.Profile.AddGold(-dataShop.numberCoinUnlock, "shop_skin_hat");
                    shopCharacter.SkinCharacterController.ChangeSkin(TypeEquipment.HAT, dataShop.idSkin);
                    GameManager.Instance.UiController.ShopCharater.ReloadLayoutShopHat();
                }
                break;
            case TypeButtonBehavior.USE:
                {
                    playerData.SetIdEquipSkin(TypeEquipment.HAT, dataShop.idSkin);
                    OnClickBtnView();
                    shopCharacter.ReloadLayoutShopHat();
                }
                break;
            case TypeButtonBehavior.REMOVE:
                {
                    playerData.SetIdEquipSkin(TypeEquipment.HAT, -1);
                    shopCharacter.SkinCharacterController.ChangeSkin(TypeEquipment.HAT, -1);
                    shopCharacter.ReloadLayoutShopHat();
                }
                break;
            default:
                break;
        }

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
                _rewardAdStatus = AdManager.Instance.ShowAdsReward(OnRewardedVideo, Helper.video_shop_hat, false);
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

    private void OnRewardedVideo(int x)
    {
        //Debug.Log("Watch Video Done");
        var playerData = GameManager.Instance.PlayerDataManager;

        int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.HAT, dataShop.idSkin);
        numberWatchVideo++;
        if (numberWatchVideo >= dataShop.numberVideoUnlock)
        {
            playerData.SetUnlockSkin(TypeEquipment.HAT, dataShop.idSkin);
        }

        playerData.SetNumberWatchVideoSkin(TypeEquipment.HAT, dataShop.idSkin, numberWatchVideo);
        shopCharacter.SkinCharacterController.ChangeSkin(TypeEquipment.HAT, dataShop.idSkin);
        GameManager.Instance.UiController.ShopCharater.ReloadLayoutShopHat();
    }
}
