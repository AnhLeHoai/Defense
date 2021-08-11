using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabTopShopCharacter : MonoBehaviour
{
    [SerializeField] private TypeTabShopCharacter type;
    [SerializeField] private Button btnTab;
    [SerializeField] private List<Sprite> listSprStateTabs;
    [SerializeField] private Image imgButton;

    private ShopCharacter shopController;
    private void Start()
    {
        btnTab.onClick.AddListener(OnClickBtnTab);
    }

    public void Init(ShopCharacter _shopController)
    {
        shopController = _shopController;
    }

    public void ActiveTab()
    {
        imgButton.sprite = listSprStateTabs[0];
    }

    public void DisableTab()
    {
        imgButton.sprite = listSprStateTabs[1];
    }

    private void OnClickBtnTab()
    {
        shopController.ActiveContentTab((int)type);

        SoundManager.Instance.PlaySoundButton();
    }
}
