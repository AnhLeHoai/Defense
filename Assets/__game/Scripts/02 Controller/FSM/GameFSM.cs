using Common.FSM;
using System.Collections;
using System.Collections.Generic;
using Firebase.Crashlytics;
using UnityEngine;

public class GameFSM : FSM
{
    public GameState CurrentGameState { get; private set; }

    private FSMState lobbyGameState;
    private LobbyAction lobbyGameAction;

    public FSMState inGameState;
    private InGameAction inGameAction;

    private FSMState endGameState;
    private EndgameAction endGameAction;

    private FSMState reviveGameState;
    private ReviveAction reviveGameAction;

    public GameFSM(GameManager gameController) : base("Game FSM")
    {
        lobbyGameState = this.AddState((byte)GameState.LOBBY);
        inGameState = this.AddState((byte)GameState.IN_GAME);
        endGameState = this.AddState((byte)GameState.END_GAME);
        reviveGameState = this.AddState((byte)GameState.REVIVE);

        lobbyGameAction = new LobbyAction(gameController, lobbyGameState);
        inGameAction = new InGameAction(gameController, inGameState);
        endGameAction = new EndgameAction(gameController, endGameState);
        reviveGameAction = new ReviveAction(gameController, reviveGameState);

        lobbyGameState.AddAction(lobbyGameAction);
        inGameState.AddAction(inGameAction);
        endGameState.AddAction(endGameAction);
        reviveGameState.AddAction(reviveGameAction);
    }

    public void ChangeState(GameState state)
    {
        //Crashlytics.Log($"Change from state {this.currentState} to {state}");
        switch (state)
        {
            case GameState.LOBBY:
                ChangeToState(lobbyGameState);
                break;
            case GameState.IN_GAME:
                ChangeToState(inGameState);
                break;
            case GameState.END_GAME:
                ChangeToState(endGameState);
                break;
            case GameState.REVIVE:
                ChangeToState(reviveGameState);
                break;
            default:
                break;
        }
    }
}
