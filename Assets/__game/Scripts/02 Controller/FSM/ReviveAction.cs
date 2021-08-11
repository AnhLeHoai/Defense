using Common.FSM;
using UnityEngine;

public class ReviveAction : FSMAction
{
    private readonly GameManager gameManager;
    private const float timeCountDown = 3;
    private float timeCountDownLeft;

    public ReviveAction(GameManager _gameController, FSMState owner) : base(owner)
    {
        gameManager = _gameController;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        timeCountDownLeft = timeCountDown;

        UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
    }

    public override void OnExit()
    {
        base.OnExit();
        SoundManager.Instance.PlayFxSound(SoundManager.GameSound.BGM);
    }
}