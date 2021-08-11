using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinCharaterController : MonoBehaviour
{
    public static readonly int MAIN_COLOR_ID = Shader.PropertyToID("_Color");
    public static readonly int EMISSION_MAP_ID = Shader.PropertyToID("_EmissionMap");
    public static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
    public static readonly int MAIN_TEXTURE_ID = Shader.PropertyToID("_MainTex");
    [Title("Character Components")]
    [SerializeField] private Character character;
    [SerializeField] private Animator animatorShop;

    [Title("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer skinnedBodyMeshRenderer;

    [Title("Skin")]
    [SerializeField] private Transform hatParentPosition;
    [SerializeField] private List<SkinnedMeshRenderer> listMeshRenderSkills;
    [SerializeField] private List<Transform> roleWeaponParent;
    [SerializeField] private List<Transform> roleSubWeaponParent;

    private MeshRenderer meshHat;
    private MeshRenderer meshFov;
    private SkinnedMeshRenderer mesSkill;
    //private WeaponUser weapon;

    //private WeaponType weaponTypeEquipped;
    private int idHat;
    private Skin skin;

    public Character Character { get => character; set => character = value; }
    public DataTextureSkin DataTextureSkin => GameManager.Instance.dataTextureSkin;
    public Color SkinColor => DataTextureSkin.dictColorSkin[skin];

    public SkinnedMeshRenderer SkinnedBodyMeshRenderer => skinnedBodyMeshRenderer;

    public MeshRenderer MeshHat => meshHat;
    public SkinnedMeshRenderer MeshSkill => mesSkill;
    //public WeaponUser Weapon => weapon;

    public Skill Skill { get; set; }

    public Skin Skin => skin;

    //public WeaponType WeaponTypeEquipped => weaponTypeEquipped;

    MaterialPropertyBlock blockMaterialMeshFov;
    MaterialPropertyBlock blockMaterialskinnedBody;

    private void Awake()
    {
        if (!character)
            return;

        if (character.fovMeshFilter)
        {
            meshFov = character.fovMeshFilter.GetComponent<MeshRenderer>();
        }

        blockMaterialMeshFov = new MaterialPropertyBlock();
        blockMaterialskinnedBody = new MaterialPropertyBlock();
    }

    public void Init(bool isMine = true)
    {
        //idHat = -1;
        if (isMine)
        {
            InitHat();
            InitSkin();
        }
        else
        {
            var typeDefault = GameManager.Instance.PlayerDataManager.GetIdSkinOtherPlayer();
            ChangeSkin(typeDefault);

            int rdShowHat = UnityEngine.Random.Range(0, 2);
            if (rdShowHat == 0)
            {
                int count = PrefabStorage.Instance.Hats.Length;
                int id = UnityEngine.Random.Range(0, count);
                ChangeHat(id);
            }
        }
    }

    public void ResetAll()
    {
        //if (character.IsPlayer)
        //{
        //    InitHat();
        //    InitSkin();
        //}
        //else
        //{
        //    ChangeSkin((int)Skin);
        //    if (meshHat)
        //        meshHat.gameObject.transform.SetParent(hatParentPosition, false);
        //}
    }

    public void CopyFrom(SkinCharaterController skin)
    {
        ChangeSkin((int)skin.skin);
        ChangeHat(skin.idHat);
        Skill = skin.Skill;
    }

    public void InitSkill()
    {
        if (!character)
            return;

        //int id = (int)character.Role;
        //RoleData roleData = GameManager.Instance.roleDatas[id];
        //var skill = roleData.Skill;
        //Skill = skill[UnityEngine.Random.Range(0, skill.Length)];
        //character.AliveAnimator.SetInteger(CharacterAction.Skill.ToAnimatorHashedKey(), (int)Skill);
        //mesSkill = listMeshRenderSkills[(int)Skill];

        SetWeapon();
    }

    private void SetWeapon()
    {
        //int skill = (int)Skill;

        //if (roleWeaponParent.Count == 0)
        //{
        //    return;
        //}

        //if (skill > roleWeaponParent.Count)
        //{
        //    Logger.LogError(new NullReferenceException($"{character.Role} với {Skill} không có weapon position"));
        //    skill = 0;
        //}

        //if (weapon)
        //{
        //    Destroy(weapon.gameObject);
        //}

        //var parentTransform = roleWeaponParent[skill];

        //RoleData roleData = GameManager.Instance.roleDatas[skill];

        //if (roleData.Weapons.Length == 0)
        //{
        //    return;
        //}

        //int levelPlayerWeaponIndex = GameManager.Instance.CurrentLevelManager.playerWeaponIndex;
        //int id = 0;
        //if (character.IsPlayer)
        //{
        //    id = levelPlayerWeaponIndex == -1
        //        ? roleData.GetIdWeaponRandom()
        //        : levelPlayerWeaponIndex;
        //}
        //else
        //{
        //    id = Random.Range(0, roleData.Weapons.Length);
        //}

        //id = Mathf.Min(id, roleData.Weapons.Length - 1);

        //weapon = Instantiate(roleData.Weapons[id], parentTransform);
        //weaponTypeEquipped = weapon.weaponType;

        //if (roleData.Weapons[id].subWeapon)
        //{
        //    var subWeaponParentTransform = roleSubWeaponParent[skill];
        //    weapon.subWeapon = Instantiate(roleData.Weapons[id].subWeapon, subWeaponParentTransform);
        //}
    }

    //public void ChangeWeapon(WeaponUser weaponUser)
    //{
    //    if (weapon)
    //    {
    //        Destroy(weapon.gameObject);
    //    }

    //    var parentTransform = roleWeaponParent[(int)Skill];
    //    weapon = Instantiate(weaponUser, parentTransform);
    //    weaponTypeEquipped = weapon.weaponType;
    //    weapon.gameObject.SetActive(false);
    //}

    private void InitHat()
    {
        var playerManager = GameManager.Instance.PlayerDataManager;
        int idSkin = playerManager.GetIdEquipSkin(TypeEquipment.HAT);
        ChangeHat(idSkin);
    }

    private void InitSkin()
    {
        var playerManager = GameManager.Instance.PlayerDataManager;
        int idSkin = playerManager.GetIdEquipSkin(TypeEquipment.SKIN);
        if (idSkin == -1)
        {
            idSkin = 0;
            playerManager.SetIdEquipSkin(TypeEquipment.SKIN, idSkin);
        }

        ChangeSkin(idSkin);
    }

    public void ChangeSkin(TypeEquipment typeEquipment, int id)
    {
        switch (typeEquipment)
        {
            case TypeEquipment.HAT:
                {
                    ChangeHat(id);
                }
                break;
            case TypeEquipment.SKIN:
                {
                    ChangeSkin(id);
                }
                break;
            case TypeEquipment.SKILL:
                {
                    // ChangeSkinAttack(id);
                }
                break;

        }
    }

    private void ChangeSkin(int id)
    {
        skin = (Skin)id;

        if (skinnedBodyMeshRenderer)
        {
            //blockMaterial = new MaterialPropertyBlock();
            if (blockMaterialskinnedBody == null)
                blockMaterialskinnedBody = new MaterialPropertyBlock();
            skinnedBodyMeshRenderer.GetPropertyBlock(blockMaterialskinnedBody);
            blockMaterialskinnedBody.SetTexture(MAIN_TEXTURE_ID, DataTextureSkin.dictTextureSkin[(Skin)id]);
            blockMaterialskinnedBody.SetColor(MAIN_COLOR_ID, Color.white);
            blockMaterialskinnedBody.SetColor(EMISSION_COLOR, Color.HSVToRGB(0, 0, 0.7f));
            skinnedBodyMeshRenderer.SetPropertyBlock(blockMaterialskinnedBody);
        }

        if (meshFov)
        {
            //var blockMaterial = new MaterialPropertyBlock();
            if (blockMaterialMeshFov == null)
                blockMaterialMeshFov = new MaterialPropertyBlock();

            meshFov.GetPropertyBlock(blockMaterialMeshFov);
            blockMaterialMeshFov.SetColor(EMISSION_COLOR, SkinColor);
            meshFov.SetPropertyBlock(blockMaterialMeshFov);
        }

    }

    private void ChangeHat(int id)
    {
        if (meshHat)
            Destroy(meshHat.gameObject);

        if (hatParentPosition == null)
            return;

        if (id < 0)
            return;

        idHat = id;
        var hat = PrefabStorage.Instance.Hats[id];
        hat = Instantiate(hat, hatParentPosition);
        meshHat = hat.GetComponent<MeshRenderer>();
    }

    private void ChangeSkinAttack(int id)
    {
        ActiveWeapon(id);
        Skill = (Skill)id;
        animatorShop.SetInteger("Skill", id);
    }

    public void DisableAllWeapon()
    {
        //for (int i = 0; i < listMeshRenderSkills.Count; i++)
        //{
        //    if (listMeshRenderSkills[i] != null)
        //    {
        //        listMeshRenderSkills[i].gameObject.SetActive(false);
        //    }
        //}

        //if (weapon)
        //{
        //    weapon.gameObject.SetActive(false);
        //}
    }

    public void ActiveWeapon()
    {
        ActiveWeapon((int)Skill);
    }

    public void ActiveWeapon(int id)
    {
        //return;
        //DisableAllWeapon();
        //if (id <= 0)
        //    return;

        //SkinnedMeshRenderer skinnedMeshRenderer = listMeshRenderSkills[id];
        //if (skinnedMeshRenderer)
        //{
        //    skinnedMeshRenderer.gameObject.SetActive(true);
        //}

        //if (weapon)
        //{
        //    weapon.gameObject.SetActive(true);
        //}
    }
}
