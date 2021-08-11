using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : GenericSingletonClass<GameController>
{
    [SerializeField]
    private Guns[] initListGuns;

    //init value of Game
    private int[] initMaxBullet = new int[6];
    private float[] initReloadTime = new float[6];
    private float[] initRange = new float[6];
    public List<int> initListGunActive = new List<int>();

    public Guns[] InitListGuns { get => initListGuns; set => initListGuns = value; }

    public void LogInGame()
    {
        Guns gun;
        for (int i = 0; i < initListGuns.Length; i++)
        {
            gun = initListGuns[i];
            gun.MaxBullets = initMaxBullet[i];
            gun.Bullet.GetComponent<Bullets>().Range = initRange[i];
            gun.ReloadTime = initReloadTime[i];
        }
    }

    public void KeepValueAfterLevel()
    {
        Guns gun;
        for (int i = 0; i < GameManager.Instance.CurrentLevelManager.Player.ListGuns.Length; i++)
        {
            gun = GameManager.Instance.CurrentLevelManager.Player.ListGuns[i].GetComponent<Guns>();
            initListGuns[i].MaxBullets = gun.MaxBullets;
            initListGuns[i].Bullet.GetComponent<Bullets>().Range = gun.Bullet.GetComponent<Bullets>().Range;
            initListGuns[i].ReloadTime = gun.ReloadTime;
        }

        for (int i = 0; i < GameManager.Instance.CurrentLevelManager.Player.listGunActive.Count; i++)
        {
            if (!initListGunActive.Contains(GameManager.Instance.CurrentLevelManager.Player.listGunActive[i]))
            {
                initListGunActive.Add(GameManager.Instance.CurrentLevelManager.Player.listGunActive[i]);
            }
        }
    }

    public void Init(GameObject[] guns)
    {
        Guns gun;
        for (int i = 0; i < initListGuns.Length; i++)
        {
            gun = guns[i].GetComponent<Guns>();
            initMaxBullet[i] = gun.MaxBullets;
            initRange[i] = gun.Bullet.GetComponent<Bullets>().Range;
            initReloadTime[i] = gun.ReloadTime;
        }
    }

    public void ResetGame()
    {
        Guns gun;
        for (int i = 0; i < initListGuns.Length; i++)
        {
            gun = initListGuns[i];
            gun.MaxBullets = initMaxBullet[i];
            gun.Bullet.GetComponent<Bullets>().Range = initRange[i];
            gun.ReloadTime = initReloadTime[i];
        }

        initListGunActive.RemoveRange(0, initListGunActive.Count);
        initListGunActive.Add(0);
    }

}
