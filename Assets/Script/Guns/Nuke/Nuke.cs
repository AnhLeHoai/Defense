using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : Guns
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

    protected override void Shooting()
    {
        GameObject projectile = SimplePool.Spawn(bullet, Player.Cam.transform.position, Player.Cam.transform.rotation);
        Vector3 shootDir = Player.Cam.transform.forward;
        projectile.GetComponent<Bomb>().SetUp(shootDir);
        currentNumberBullets--;
    }
}
