using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewWeaponPrice : Price, IUpdatePrice
{
    private int index;

    public int Index { get => index; set => index = value; }

    public override void UpdatePrice()
    {
        GameManager.Instance.CurrentLevelManager.Player.listGunActive.Add(index);
    }

    public override void Operation()
    {
        index = FindADeactivateGun();
        Gun = GameManager.Instance.CurrentLevelManager.Player.ListGuns[index];
    }
}
