using DG.Tweening;
using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLose : UICanvas
{
    [SerializeField] private Button btnRetry;
    [SerializeField] private Button btnSkipLevel;
    [SerializeField] private Image imgSkipLevelBg;
    private Tween tween;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        btnRetry.onClick.AddListener(OnClickBtnRetry);
        btnSkipLevel.onClick.AddListener(OnClickBtnRevive);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);

        if (!isShow)
        {
            return;
        }
        if (tween != null)
            tween.Kill();

        btnSkipLevel.gameObject.SetActive(true);
        imgSkipLevelBg.fillAmount = 0;
        tween = imgSkipLevelBg.DOFillAmount(1, Constants.REVIVE_CHOOSING_TIME).SetEase(Ease.Linear).OnComplete(() => { btnSkipLevel.gameObject.SetActive(false); });
    }

    private void OnClickBtnRetry()
    {
        OnBackPressed();

        GameManager.Instance.ShowInterAdsEndGame("end_game_lose");
        GameManager.Instance.CurrentLevelManager.Player.ResetValueAtBeginning();
        Round.roundIndex = 0;
        GameManager.Instance.LoadLevel();
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnRevive()
    {
        if (imgSkipLevelBg.fillAmount == 1)
        {
            return;
        }

#if UNITY_EDITOR

        OnReward(1);
#else
        RewardAdStatus adStatus = AdManager.Instance.ShowAdsReward((x) =>
        {
            OnReward(1);

        }, Helper.video_reward_revive, true);

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

    private void OnReward(int x)
    {
        if (x <= 0 && !isShow)
            return;
        StartCoroutine(IEWaitRevive());

    }

    private IEnumerator IEWaitRevive()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.Revive();
        OnBackPressed();
    }
}
