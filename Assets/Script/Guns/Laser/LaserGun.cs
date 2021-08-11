using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Guns
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
                shootAnim.SetActive(true);
                StartCoroutine(WaitMaxTimeShoot());
            }
            if (Input.GetMouseButtonUp(0))
            {
                shootAnim.SetActive(false);
                Shooting();
            }
        }
        else
        {
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

    IEnumerator WaitMaxTimeShoot()
    {
        yield return new WaitForSeconds(5);
        shootAnim.SetActive(false);
        Shooting();
    }
    protected override void Shooting()
    {
        currentNumberBullets--;
    }
}
