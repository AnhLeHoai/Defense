using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShowRewards : UICanvas
{
    [SerializeField] private Image iconRewardBg;
    [SerializeField] private Image iconReward;
    [SerializeField] private Image imgProcessReward;
    [SerializeField] private Text txtProcessReward;
    [SerializeField] private Text txtCoinReward;
    [SerializeField] private DataRewardEndGame dataRewards;
    private int percentReward;

    private Tweener tweenCoin;
    private int tmpPercent;

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (isShow)
        {
            SetupRewardEndGame();
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
        iconReward.SetNativeSize();
        iconRewardBg.sprite = iconReward.sprite;
        iconRewardBg.SetNativeSize();

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
                //StartCoroutine(IEShowRateGame());
                PlayerPrefs.SetInt("showRate", 1);
            }
        }

        float ratio = (float)process / (float)reward.NumberWin;
        imgProcessReward.fillAmount = 0;
        imgProcessReward.DOFillAmount(ratio, 1f);

        iconReward.fillAmount = 0;
        iconReward.DOFillAmount(ratio, 1f);

        percentReward = (int)(ratio * 100);
        SetPercentReward(percentReward);
    }

    private IEnumerator IEWaitShowRewardEndGame(RewardEndGame reward)
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.END_GAME);
    }

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

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        
    }
}
