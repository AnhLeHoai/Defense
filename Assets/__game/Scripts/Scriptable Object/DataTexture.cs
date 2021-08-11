using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataTexture", menuName = "ScriptableObjects/Data Texture")]
public class DataTexture : SerializedScriptableObject
{

    [SerializeField] private List<Sprite> ListIconHats;
    [SerializeField] private List<Sprite> ListIconSkins;
    [SerializeField] private List<Sprite> ListIconPets;
    [SerializeField] private List<Sprite> ListIconSkills;
    public List<Sprite> ListSprBgItemShops;
    public Sprite IconCoin;
    public Sprite IconCoinLuckyWheel;

    [SerializeField] private List<Sprite> ListIconSprKey;

    public Sprite GetIconHat(int id)
    {
        if (id < ListIconHats.Count)
            return ListIconHats[id];

        return null;
    }

    public Sprite GetIconSkin(int id)
    {
        if (id < ListIconSkins.Count)
            return ListIconSkins[id];

        return null;
    }

    public Sprite GetIconPet(int id)
    {
        if (id < ListIconPets.Count)
            return ListIconPets[id];

        return null;
    }

    public Sprite GetIconSkill(int id)
    {
        if (id == -1)
            id = 0;

        if (id < ListIconSkills.Count)
            return ListIconSkills[id];

        return null;
    }

    public Sprite GetIconKey(bool isActive)
    {
        int index = isActive ? 0 : 1;
        return ListIconSprKey[index];
    }


}
