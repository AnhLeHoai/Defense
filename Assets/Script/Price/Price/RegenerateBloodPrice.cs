using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateBloodPrice : Price, IUpdatePrice
{
    private float heartRate;

    public float HeartRate { get => heartRate; set => heartRate = value; }
    public override void UpdatePrice()
    {
        Heart.heartRate += heartRate;
    }

    public override void Operation()
    {
        if (Heart.heartRate == 100)
        {
            HeartRate = 0;
        }
        else if (Heart.heartRate >= 95)
        {
            HeartRate = 100 - Heart.heartRate;
        }
        else
        {
            HeartRate = 5;
        }
    }
    //protected override void SetPriceText()
    //{
    //    Text.text = "Regenerate Blood";
    //}
}
