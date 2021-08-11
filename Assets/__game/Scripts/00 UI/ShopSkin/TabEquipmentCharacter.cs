using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class TabEquipmentCharacter : MonoBehaviour
{
    [SerializeField] private List<RectTransform> listPageItems;
    [SerializeField] private RectTransform rectTransParent;
    [SerializeField] private HorizontalScrollSnap_Lobby scrollViewController;

    public int currentIndex;
    private int maxIndex;

    private void Start()
    {
        if (scrollViewController)
            scrollViewController.OnSelectionPageChangedEvent.AddListener(ChangeTab);
    }
    // Start is called before the first frame update
    public virtual void Init(List<DataShop> listData, bool isRestPosTab = true)
    {
        if (isRestPosTab)
        {
            rectTransParent.anchoredPosition = Vector3.zero;
            currentIndex = 0;
            maxIndex = listPageItems.Count;
        }

    }

    public void Reset()
    {
        rectTransParent.anchoredPosition = Vector3.zero;
        currentIndex = 0;

        GameManager.Instance.UiController.ShopCharater.SetupTabBottom(currentIndex, listPageItems.Count);
    }


    private void ChangeTab(int index)
    {
        if (currentIndex == index)
            return;

        currentIndex = index;
        GameManager.Instance.UiController.ShopCharater.SetupTabBottom(currentIndex, listPageItems.Count);
    }
}
