using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [Title("Map size")]
    [SerializeField] private Vector3 mapCenter = new Vector3(0, 0);
    [SerializeField] private float mapSize = 48.62f;

    public LevelResult Result { get; private set; }
    public Vector3 MapCenter => mapCenter;
    public float MapSize => mapSize;

    public Player Player { get => player; set => player = value; }
    public Canavas CanavasController { get => canavasController; set => canavasController = value; }

    [SerializeField]
    private Player player;
    [SerializeField]
    private Canavas canavasController;

    protected virtual void Start()
    {
        SetUpLevelEnvironment();
        GameManager.Instance.OnLevelFinishedLoading(this);
        var skyboxes = PrefabStorage.Instance.skyboxes;
        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];
        GameManager.Instance.UiController.UiMainLobby.BtnPlay.gameObject.SetActive(true);
    }

    private void SetUpLevelEnvironment()
    {
        ResetLevelState();
    }

    private void ResetLevelState()
    {
        Result = LevelResult.NotDecided;
    }

    public virtual void StartLevel()
    {
        player.gameObject.SetActive(true);
        player.StartLevel();
        CanavasController.StartCanavaGame();
        ManagerEnemy.Active = true;
        ManagerEnemy.FirstTimeActive = true;
    }
    
    public LevelResult CheckGameResult()
    {
        if (Result != LevelResult.NotDecided)
        {
            return Result;
        }

        // TODO: Check game result
        
        return LevelResult.NotDecided;
    }

    public void EndGame(LevelResult levelResult)
    {
        Result = levelResult;
        GameManager.Instance.DelayedEndgame(levelResult);
    }

    public void Revive()
    {
        Result = LevelResult.NotDecided;
        StartLevel();
        Heart.heartRate = 50;
        GameManager.Instance.LoadLevel();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mapCenter, new Vector3(mapSize, 2.9f, mapSize));
        Gizmos.color = Color.white;
    }
    
#endif
}
