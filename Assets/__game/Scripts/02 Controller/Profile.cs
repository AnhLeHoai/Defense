using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{
    public Profile()
    {
        if (!GameManager.Instance.PlayerDataManager.GetUnlockSkin(TypeEquipment.SKILL, (int)Skill.JUMP_AND_EAT))
        {
            GameManager.Instance.PlayerDataManager.SetUnlockSkin(TypeEquipment.SKILL, (int)Skill.JUMP_AND_EAT);
        }

        if (GameManager.Instance.PlayerDataManager.GetIdEquipSkin(TypeEquipment.SKILL) == -1)
        {
            GameManager.Instance.PlayerDataManager.SetIdEquipSkin(TypeEquipment.SKILL, (int)Skill.JUMP_AND_EAT);
        }
    }

    public int GetNumberPlay()
    {
        return GameManager.Instance.PlayerDataManager.GetNumberPlay();
    }

    public void SetNumberPlay(int num)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;
        playerdata.SetNumberPlay(num);
    }

    public void AddGold(int goldBonus, string _analytic)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;
        int _count = GetGold() + goldBonus;
        playerdata.SetGold(_count);

        if (playerdata.actionUITop != null)
        {
            playerdata.actionUITop(TypeItem.Coin);
        }
    }

    public int GetGold()
    {
        return GameManager.Instance.PlayerDataManager.GetGold();
    }

    public void AddKey(int amount, string _analytic)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;

        playerdata.SetKey(GetKey() + amount);

        if (playerdata.actionUITop != null && amount == 1)
        {
            playerdata.actionUITop(TypeItem.Key);
        }
    }

    public int GetKey()
    {
        return GameManager.Instance.PlayerDataManager.GetKey();
    }
}
