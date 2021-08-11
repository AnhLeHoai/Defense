using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : Guns
{
    // Start is called before the first frame update
    void Start()
    {
        currentNumberBullets = maxBullets;
        CurrentReloadTime = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        Operation();
    }

    protected override void Operation()
    {
        if (CurrentNumberBullets >= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Shooting();
            }
            if(Input.GetMouseButtonUp(0))
            {
                shootAnim.SetActive(false);
            }
        }
        else
        {
            shootAnim.SetActive(false);
            if (CurrentReloadTime <= 0)
            {
                CurrentReloadTime = reloadTime;
                CurrentNumberBullets = MaxBullets;
            }
            else
            {
                CurrentReloadTime -= Time.deltaTime;
            }
        }
    }
    protected override void Shooting()
    {
        shootAnim.SetActive(true);
        GameObject projectile = SimplePool.Spawn(bullet, Player.Cam.transform.position, Quaternion.identity);
        Vector3 shootDir = Player.Cam.transform.forward;
        projectile.GetComponent<BurstBullet>().SetUp(shootDir);
        currentNumberBullets--;
    }
}
