using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceReloadTimePrice : Price, IUpdatePrice
{
    [SerializeField]
    private float reduceTime;

    public float ReduceTime { get => reduceTime; set => reduceTime = value; }

    public override void UpdatePrice()
    {
        Gun = FindAnActivateGun();
        Gun.GetComponent<Guns>().ReloadTime -= reduceTime;

    }

    public override void Operation()
    {
        Gun = FindAnActivateGun();
    }

    //protected override void SetPriceText()
    //{
    //    Text.text = "Reduce Reoad Time:\n" + Gun.name;
    //}
}
