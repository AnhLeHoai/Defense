using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TabHat : TabEquipmentCharacter
{
    [SerializeField] private List<ItemHat> listItemHats;


    public override void Init(List<DataShop> listData, bool isRestPosTab = true)
    {
        base.Init(listData, isRestPosTab);


        for (int i = 0; i < listData.Count; i++)
        {
            listItemHats[i].Init(listData[i]);
        }
    }
}
