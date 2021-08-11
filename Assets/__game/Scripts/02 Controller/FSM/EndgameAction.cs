using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameAction : FSMAction
{
    private readonly GameManager gameManager;

    public EndgameAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        SoundManager.Instance.StopFootStep();
        gameManager.UiController.ActiveTutHand(false);
        gameManager.PlayerDataManager.ClearListIdSkin();

        int gold = 0;
        if (gameManager.CurrentLevelManager.Result == LevelResult.Win)
        {
            gold += Constants.GOLD_WIN;
        }

        ProcessWinLose(gold);

        SoundManager.Instance.PlayFxSound(gameManager.CurrentLevelManager.Result);
    }

    private void ProcessWinLose(int gold)
    {
        switch (gameManager.CurrentLevelManager.Result)
        {
            case LevelResult.Win:

                gameManager.UiController.OpenUiWin(gold);

                Analytics.LogEndGameWin(GameManager.Instance.CurrentLevel);
                gameManager.SpawnEffWin();
                break;
            case LevelResult.Lose:
                gameManager.UiController.OpenUiLose();

                Analytics.LogEndGameLose(GameManager.Instance.CurrentLevel);
                break;
            default:
                break;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        SoundManager.Instance.StopSound(SoundManager.GameSound.BGM);
    }
}
