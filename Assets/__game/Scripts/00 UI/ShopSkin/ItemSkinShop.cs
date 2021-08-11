using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkinShop : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected List<Button> listBtnBehavior;
    [SerializeField] private Button btnView;
    [SerializeField] protected Text txtVideo;
    [SerializeField] private Text txtCoin;
    private Image imgBg;
    [SerializeField]
    protected Image ImgBg
    {
        get
        {
            if (imgBg == null)
                imgBg = GetComponent<Image>();

            return imgBg;
        }
    }

    protected DataShop dataShop;
    protected ShopCharacter shopCharacter;


    // Start is called before the first frame update
    void Start()
    {

        btnView.onClick.AddListener(OnClickBtnView);
        for (int i = 0; i < listBtnBehavior.Count; i++)
        {
            int id = i;
            listBtnBehavior[i].onClick.AddListener(() => OnClickBtnBehaviour(id));
        }
        shopCharacter = GameManager.Instance.UiController.ShopCharater;
    }

    public virtual void Init(DataShop data)
    {
        dataShop = data;
        //icon.sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconSkin(data.idSkin);
        icon.SetNativeSize();

        for (int i = 0; i < listBtnBehavior.Count; i++)
        {
            listBtnBehavior[i].gameObject.SetActive(false);
        }

        txtCoin.text = data.numberCoinUnlock.ToString();


    }

    protected virtual void OnClickBtnView()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    protected virtual void OnClickBtnBehaviour(int idBehaviour)
    {
        SoundManager.Instance.PlaySoundButton();
    }
}


public enum TypeButtonBehavior
{
    SPIN = 0,
    UNLOCK_BY_VIDEO = 2,
    UNLOCK_BY_COIN = 1,
    USE = 4,
    REMOVE = 3
}
