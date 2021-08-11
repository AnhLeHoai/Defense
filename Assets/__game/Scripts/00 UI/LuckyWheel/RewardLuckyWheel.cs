using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardLuckyWheel : MonoBehaviour
{
    [SerializeField] private Image iconReward;
    [SerializeField] private Text txtNumberCoin;
    private Transform transIcon;
    private Transform tranNumberCoin;

    private void Start()
    {
        tranNumberCoin = txtNumberCoin.GetComponent<Transform>();
        transIcon = iconReward.GetComponent<Transform>();
    }

    public void Init(DataRewardLuckyWheel dataLuckyWheel)
    {
        var playerData = GameManager.Instance.PlayerDataManager;
        txtNumberCoin.gameObject.SetActive(true);
        switch (dataLuckyWheel.Type)
        {
            case TypeGift.GOLD:
                {
                    iconReward.sprite = playerData.DataTexture.IconCoinLuckyWheel;
                    txtNumberCoin.text = dataLuckyWheel.Amount.ToString();
                }
                break;

            case TypeGift.HAT:
                {
                    if (playerData.GetUnlockSkin(TypeEquipment.HAT, dataLuckyWheel.IdType))
                    {
                        iconReward.sprite = playerData.DataTexture.IconCoinLuckyWheel;
                        txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                    }
                    else
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconHat(dataLuckyWheel.IdType);
                        txtNumberCoin.gameObject.SetActive(false);
                    }
                }
                break;
            case TypeGift.SKIN:
                {
                    if (playerData.GetUnlockSkin(TypeEquipment.SKIN, dataLuckyWheel.IdType))
                    {
                        iconReward.sprite = playerData.DataTexture.IconCoin;
                        txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                    }
                    else
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconSkin(dataLuckyWheel.IdType);
                        txtNumberCoin.gameObject.SetActive(false);
                    }
                }
                break;
            case TypeGift.PET:
                {
                    if (playerData.GetUnlockSkin(TypeEquipment.PET, dataLuckyWheel.IdType))
                    {
                        iconReward.sprite = playerData.DataTexture.IconCoin;
                        txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                    }
                    else
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconPet(dataLuckyWheel.IdType);
                        txtNumberCoin.gameObject.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }

        iconReward.SetNativeSize();
    }

    private void Update()
    {
        //v3Rotate.z = -(_transWheel.eulerAngles.z + angle);
        tranNumberCoin.rotation = Quaternion.identity;
        transIcon.rotation = Quaternion.identity;

    }
}
