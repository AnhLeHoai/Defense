using Common.FSM;
using UnityEngine;

public class LobbyAction : FSMAction
{
    private readonly GameManager gameManager;

    public LobbyAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        SoundManager.Instance.PlayFxSound(soundEnum: SoundManager.GameSound.Lobby);
        if (gameManager.Profile.GetKey() >= 3)
        {
            ShowChestKey();
        }

    }

    public override void OnExit()
    {
        base.OnExit();
        SoundManager.Instance.StopSound(SoundManager.GameSound.Lobby);
    }

  

    private void ShowChestKey()
    {

        var playerData = GameManager.Instance.PlayerDataManager;
        int indexReward = playerData.GetCurrentIndexRewardEndGame();
        if (indexReward >= playerData.DataRewardEndGame.Datas.Count)
        {
            indexReward = playerData.DataRewardEndGame.Datas.Count - 1;
        }

        var reward = playerData.DataRewardEndGame.Datas[indexReward];

        GameManager.Instance.UiController.OpenPopupChestKey(reward);

    }
}