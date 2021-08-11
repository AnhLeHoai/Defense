using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreRangePrice : Price, IUpdatePrice
{
    private float moreRange;

    public float MoreRange { get => moreRange; set => moreRange = value; }

    public override void UpdatePrice()
    {
        Gun.GetComponent<Guns>().Bullet.GetComponent<Bullets>().Range += moreRange;
    }

    public override void Operation()
    {
        Gun = FindAnActivateGun();
        moreRange = Gun.GetComponent<Guns>().Bullet.GetComponent<Bullets>().Range / 10;
    }
}
