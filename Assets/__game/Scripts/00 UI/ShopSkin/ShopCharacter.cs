using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopCharacter : UICanvas
{
    [SerializeField] private List<TabEquipmentCharacter> listContents;
    [SerializeField] private List<TabTopShopCharacter> listTabsTop;
    public DataShopSkin dataShopSkin;
    [FormerlySerializedAs("skinCharaterController")] [SerializeField] private SkinCharacterController skinCharacterController;
    [SerializeField] private Transform transPets;
    private Dictionary<int, PetController> dictObjPets = new Dictionary<int, PetController>();
    private PetController currentPet;

    public SkinCharacterController SkinCharacterController { get => skinCharacterController; }
    [SerializeField] private List<ElementTabBottom> listObjTabBottom;
    [SerializeField] private GameObject objNotEnoughtGold;

    private void Init()
    {
        listContents[0].Init(dataShopSkin.listDataShopHats);
        listContents[1].Init(dataShopSkin.listDataShopSkins);
        listContents[2].Init(dataShopSkin.listDataShopPets);
        //listContents[3].Init(dataShopSkin.listDataShopSkills);

        for (int i = 0; i < listTabsTop.Count; i++)
        {
            listTabsTop[i].Init(this);
        }

        ActiveContentTab(0);
        skinCharacterController.Init();
        skinCharacterController.animatorShop.SetInteger(
                CharacterAction.RandomValue.ToAnimatorHashedKey(),
                Random.Range(0, Character.TotalIdlePoses));
        InitPet();
    }

    public void ReloadLayoutShopHat()
    {
        listContents[0].Init(dataShopSkin.listDataShopHats, false);
    }

    public void ReloadLayoutShopSkin()
    {
        listContents[1].Init(dataShopSkin.listDataShopSkins, false);
    }

    public void ReloadLayoutShopSkill()
    {
        listContents[3].Init(dataShopSkin.listDataShopSkills, false);
    }

    public void ReloadLayoutShopPet()
    {
        listContents[2].Init(dataShopSkin.listDataShopPets, false);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (isShow)
        {
            Init();
        }
    }

    public void ActiveContentTab(int idTab)
    {
        for (int i = 0; i < listContents.Count; i++)
        {
            listContents[i].gameObject.SetActive(false);
            listTabsTop[i].DisableTab();
        }

        listContents[idTab].gameObject.SetActive(true);
        listContents[idTab].Reset();

        listTabsTop[idTab].ActiveTab();

        skinCharacterController.ChangeSkin(TypeEquipment.SKILL, -1);
    }

    public void ChangePet(int id)
    {
        foreach (var item in dictObjPets)
        {
            item.Value.gameObject.SetActive(false);
        }

        if (dictObjPets.ContainsKey(id))
        {
            currentPet = dictObjPets[id];
            currentPet.gameObject.SetActive(true);
            MakePetDoActionRepeating();
        }
        else
        {
            var gPet = Resources.Load<PetController>("Pets/" + (TypePet)id);
            var pet = Instantiate(gPet, transPets);
            pet.gameObject.SetActive(true);
            currentPet = pet;
            dictObjPets.Add(id, pet);
            MakePetDoActionRepeating();
        }
    }

    public void InitPet()
    {
        int id = GameManager.Instance.PlayerDataManager.GetIdEquipSkin(TypeEquipment.PET);
        foreach (var item in dictObjPets)
        {
            item.Value.gameObject.SetActive(false);
        }

        if (id != -1)
        {
            if (dictObjPets.ContainsKey(id))
            {
                dictObjPets[id].gameObject.SetActive(true);
                currentPet = dictObjPets[id];
                MakePetDoActionRepeating();
            }
            else
            {
                var gPet = Resources.Load<PetController>("Pets/" + (TypePet)id);
                var pet = Instantiate(gPet, transPets);
                dictObjPets.Add(id, pet);
            }
        }
    }

    public void MakePetDoActionRepeating()
    {
        CancelInvoke();
        InvokeRepeating(nameof(MakePetDoAction), 10f, 5f);
    }

    public void MakePetDoAction()
    {
        if (currentPet)
        {
            currentPet.Animator.SetTrigger(CharacterAction.Action.ToAnimatorHashedKey());
        }
    }

    public void SetupTabBottom(int currentTab, int countTab)
    {
        for (int i = 0; i < listObjTabBottom.Count; i++)
        {
            listObjTabBottom[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < countTab; i++)
        {
            listObjTabBottom[i].gameObject.SetActive(true);
            listObjTabBottom[i].Setup(false);

        }

        listObjTabBottom[currentTab].Setup(true);
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        SoundManager.Instance.PlaySoundButton();
    }

    public void ActiveNotiNotEnoughtGold(Transform trans)
    {
        SetParentNotiNotEnoughtGold(trans);
        objNotEnoughtGold.SetActive(true);
    }

    public void DisableNotiNotEnoughtGold()
    {
        objNotEnoughtGold.SetActive(false);
    }

    private void SetParentNotiNotEnoughtGold(Transform trans)
    {
        objNotEnoughtGold.transform.SetParent(trans);
        objNotEnoughtGold.transform.localPosition = Vector3.zero;
    }
}
