using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManagerPrice : MonoBehaviour
{
    public void UpdatePrice()
    {
        GameObject tmpBtn = EventSystem.current.currentSelectedGameObject;
        Price price = tmpBtn.GetComponent<PriceButton>().Price;
        IUpdatePrice updatePrice = price.GetComponent<IUpdatePrice>();
        updatePrice.UpdatePrice();
        GameController.Instance.KeepValueAfterLevel();
    }
}
