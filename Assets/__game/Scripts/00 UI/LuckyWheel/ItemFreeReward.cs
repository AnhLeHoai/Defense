using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFreeReward : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField] private Text txtNumberSpin;
    //[SerializeField] private GameObject ObjTick;
    [SerializeField] private Image imgBg;
    [SerializeField] private List<Sprite> listSprBg;


    private float WidthBar = 630;

    public void Init(RewardEndGame data)
    {
        txtNumberSpin.text = data.NumberWin.ToString();
        bool unlock = GameManager.Instance.PlayerDataManager.GetUnlockSkin(data.Type, data.Id);
        if (!unlock)
        {
            int numberWatchVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchVideoSpin();
            if (data.NumberWin <= numberWatchVideo)
            {
                
                unlock = true;

                Debug.Log("Year nhan thuowng");

                RewardEndGame reward = new RewardEndGame();
                reward.Type = data.Type;
                reward.Id = data.Id;

                GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                GameManager.Instance.PlayerDataManager.SetUnlockSkin(data.Type, data.Id);

            }
        }

        //ObjTick.SetActive(unlock);
        int indexBg = unlock ? 0 : 1;
        imgBg.sprite = listSprBg[indexBg];
        switch (data.Type)
        {
            case TypeEquipment.HAT:
                icon.sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconHat(data.Id);
                break;
            case TypeEquipment.SKIN:
                icon.sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconSkin(data.Id);
                break;
            case TypeEquipment.PET:
                icon.sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconPet(data.Id);
                break;
            case TypeEquipment.SKILL:
                icon.sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconSkill(data.Id);
                break;
            default:
                break;
        }

        icon.SetNativeSize();
        float ratio = (float)data.NumberWin / 20f;
        var v3 = new Vector3(ratio * WidthBar, 10, 0);
        this.GetComponent<RectTransform>().anchoredPosition = v3;
    }
}
