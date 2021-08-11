using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabContentSkin : TabEquipmentCharacter
{
    [SerializeField] private List<ItemSkin> listItems;


    public override void Init(List<DataShop> listData, bool isRestPosTab = true)
    {
        base.Init(listData, isRestPosTab);
        for (int i = 0; i < listData.Count; i++)
        {
            listItems[i].Init(listData[i]);
        }
    }
}
