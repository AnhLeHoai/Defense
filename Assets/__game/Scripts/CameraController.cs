using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    public Camera Camera => camera;

    public void Init()
    {
    }
    
    // Không cần dùng thì xóa nhé
    //public void SetupCamera(Action done = null)
    //{
    //    var levelManager = GameManager.Instance.CurrentLevelManager;
    //    var player = levelManager.Player;

    //    var cameraPosition = player ? player.transform.position : levelManager.MapCenter;
    //    cameraPosition.y += 12;
    //    cameraPosition.z -= 15;
        

    //    Vector3 cameraStartPosition = Vector3.zero;
    //    cameraStartPosition.x = levelManager.MapCenter.x;
    //    cameraStartPosition.y = levelManager.MapSize * 2;
    //    cameraStartPosition.z = -levelManager.MapSize * 2;
    //    transform.position = cameraStartPosition;
    //    transform.eulerAngles = new Vector3(45, 0, 0);

    //    transform.DOMove(cameraPosition, 1f).OnComplete(() => { done?.Invoke(); });
    //    transform.DORotate(new Vector3(26, 0, 0), 1);
    //}
}
