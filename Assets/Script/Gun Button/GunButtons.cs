using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunButtons : MonoBehaviour
{
    [SerializeField]
    private int index;

    private Guns gun;
    [SerializeField]
    public TextMeshProUGUI bulletsText;

    private float currentReloadTime;
    public Guns Gun { get => gun; set => gun = value; }
    public int Index { get => index; set => index = value; }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentLevelManager.Player.currentGunActive == GameManager.Instance.CurrentLevelManager.Player.listGunActive[index])
        {
            if (gun.CurrentNumberBullets > 0)
            {
                bulletsText.text = gun.name + "\n" + gun.CurrentNumberBullets;
                currentReloadTime = gun.ReloadTime;
            }
            else
            {
                bulletsText.text = "Reload time:" + ((int)gun.CurrentReloadTime +1);
                currentReloadTime = gun.CurrentReloadTime;
            }
        }
        else
        {
            if (gun.CurrentNumberBullets > 0)
            {
                bulletsText.text = gun.name + "\n" + (int)gun.CurrentNumberBullets;
            }
            else
            {
                currentReloadTime -= Time.deltaTime;
                bulletsText.text = "Reload time:" + ((int)currentReloadTime + 1);
                if ( currentReloadTime <= 0)
                {
                    gun.CurrentNumberBullets = gun.MaxBullets;
                }
            }
 
        }
    }

    public void SetGun( int index)
    {
        gun = GameManager.Instance.CurrentLevelManager.Player.ListGuns[index].GetComponent<Guns>();
    }

    public void ResetGun()
    {
        gun = null;
    }
}
