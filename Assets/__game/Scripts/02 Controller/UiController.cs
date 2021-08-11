using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public UiMainLobby UiMainLobby;
    public UiLose UiLose;
    public UiWin UiWin;
    public ShopCharacter ShopCharater;
    public ShopIAP ShopIap;
    public UiTop UiTop;
    //public UiLeaderBoardIngame UiLeaderboard;
    public PopupRewardEndGame PopupRewardEndGame;
    public PopupChestKey PopupChestKey;
    public LuckyWheel LuckeyWheel;
    public GameObject Loading;
    public GameObject ObjTutHand;
    //public PopupShowRewards PopupShowRewards;

    public GameObject ObjJoyStick;

    public void Init()
    {
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
    }

    public void OpenUiLose()
    {
        UiLose.Show(true);
    }

    public void OpenUiWin(int gold)
    {
        UiWin.Show(true);
        UiWin.Init(gold);
    }

    public void OpenShopCharacter()
    {
        ShopCharater.Show(true);
    }

    public void OpenShopIap()
    {
        ShopIap.Show(true);
    }

    public void OpenPopupReward(RewardEndGame reward, TypeDialogReward type)
    {
        if (PopupRewardEndGame.IsShow)
            return;

        PopupRewardEndGame.Show(true);
        PopupRewardEndGame.Init(reward, type);
    }

    public void OpenPopupChestKey(RewardEndGame reward)
    {
        PopupChestKey.Show(true);
        PopupChestKey.Init(reward);
    }

    public void OpenLuckyWheel()
    {
        LuckeyWheel.Show(true);
    }

    public void OpenLoading(bool isLoading)
    {
        Loading.SetActive(isLoading);
    }

    public void ActiveTutHand(bool isActive)
    {
        ObjTutHand.SetActive(isActive);
    }

}

