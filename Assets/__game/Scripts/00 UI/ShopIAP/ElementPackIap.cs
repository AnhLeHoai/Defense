using RocketTeam.Sdk.Services.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementPackIap : MonoBehaviour
{
    [SerializeField] private Text txtAmount;
    [SerializeField] private Button btnBuy;
    [SerializeField] private Text txtPrice;
    [SerializeField] private IdPack id;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnClickBtnBuy);
    }

    public void Init()
    {
        var iapController = GameManager.Instance.IapController;

        if (id != IdPack.NO_ADS_PREMIUM && id != IdPack.NO_ADS_BASIC)
        {
            txtAmount.text = string.Format("+{0}", iapController.Data.dictInfoPackage[id].listRewardPack[0].amount);
        }
        else
        {
            if (GameManager.Instance.PlayerDataManager.IsNoAds())
            {
                btnBuy.interactable = false;
            }
        }

        txtPrice.text = iapController.DictInfoPricePack[id].localizedPrice;
    }

    private void OnClickBtnBuy()
    {
        GameManager.Instance.IapController.EventPurchaseSuccess = OnPurchaseSucess;
        GameManager.Instance.IapController.PurchaseProduct((int)id);

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnPurchaseSucess(IdPack _idpack, bool _isSuccess)
    {
        if (_idpack == IdPack.NO_ADS_BASIC || _idpack == IdPack.NO_ADS_PREMIUM)
        {
            //AdManager.Instance.HideBanner();
            btnBuy.interactable = false;
        }

    }
}
