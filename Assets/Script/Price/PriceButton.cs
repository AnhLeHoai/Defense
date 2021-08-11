using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PriceButton : MonoBehaviour
{
    private int indexPrice;
    private Price price;
    [SerializeField]
    private TextMeshProUGUI text;

    public Price Price { get => price; set => price = value; }
    public int IndexPrice { get => indexPrice; set => indexPrice = value; }

    // Update is called once per frame
    void Update()
    {
        if ( price != null)
        {
            SetText();
        }
    }

    public void SetText()
    {
        text.text = price.name +"\n" + price.Gun.name;
    }
}
