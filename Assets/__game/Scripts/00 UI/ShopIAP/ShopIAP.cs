using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopIAP : UICanvas
{
    [SerializeField] private List<ElementPackIap> listElementPackIap;


    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (IsShow)
            Init();
    }

    public void Init()
    {
        for (int i = 0; i < listElementPackIap.Count; i++)
        {
            listElementPackIap[i].Init();
        }
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();

        SoundManager.Instance.PlaySoundButton();
    }
}
