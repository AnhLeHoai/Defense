using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPrices : MonoBehaviour
{
    [SerializeField]
    private List<Price> priceList = new List<Price>();
    [SerializeField]
    private List<PriceButton> priceButtons;

    private static bool activate = false;

    public List<Price> PriceList { get => priceList; set => priceList = value; }
    public List<PriceButton> PriceButtons { get => priceButtons; set => priceButtons = value; }
    public static bool Activate { get => activate; set => activate = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            for (int i = 0; i < priceButtons.Count; i++)
            {
                int tmp = RotatePrice();
                priceButtons[i].Price = priceList[tmp];
                priceButtons[i].Price.Operation();
                priceButtons[i].IndexPrice = tmp;
            }
            Activate = false;
        }
        //else
        //{
        //    for ( int i = 0; i < priceList.Count; i++)
        //    {
        //        if ( priceList[i].Activate)
        //        {
        //            return;
        //        }
        //    }
        //    ResetPrice();
        //}
    }

    public int RotatePrice()
    {
        int attempt = 0;
        if (GameManager.Instance.CurrentLevelManager.Player.listGunActive.Count >= 3)
        {
            priceList[3].Activate = true;
        }
        int tmp;
        do
        {
            attempt += 1;
            tmp = Random.Range(0, priceList.Count - 1);
            
            if ( attempt >= 50)
            {
                Debug.LogError("No price satisfied!!!");
            }
        } while (priceList[tmp].Activate);
        priceList[tmp].Activate = true;
        return tmp;
    }

    public void ResetPrice()
    {
        for ( int i = 0; i < priceList.Count; i++)
        {
            priceList[i].Activate = false;
        }
    }

    public void TurnOffPriceButon()
    {
        for (int i = 0; i < priceButtons.Count; i++)
        {
            priceButtons[i].gameObject.SetActive(false);
        }
    }

    public void TurnOnPriceButon()
    {
        for (int i = 0; i < priceButtons.Count; i++)
        {
            priceButtons[i].gameObject.SetActive(true);
        }
    }
}
