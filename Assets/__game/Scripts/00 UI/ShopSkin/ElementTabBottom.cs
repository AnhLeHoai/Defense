using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementTabBottom : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private List<Sprite> listSprTab;

    public void Setup(bool isActiveTab)
    {
        int index = isActiveTab ? 0 : 1;
        imgIcon.sprite = listSprTab[index];

    }
}
