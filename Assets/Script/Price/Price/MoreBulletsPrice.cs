using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreBulletsPrice : Price, IUpdatePrice
{
    private int moreBullets;

    public int MoreBullets { get => moreBullets; set => moreBullets = value; }

    public override void UpdatePrice()
    {
        Gun.GetComponent<Guns>().MaxBullets += moreBullets;
    }

    public override void Operation()
    {
        Gun = FindAnActivateGun();
        if (Gun.GetComponent<Guns>().MaxBullets >= 10)
        {
            MoreBullets = 100;
        }
        else
        {
            MoreBullets = 1;
        }
    }
}
