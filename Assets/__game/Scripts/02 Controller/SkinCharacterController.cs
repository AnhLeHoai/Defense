using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCharacterController : MonoBehaviour
{
    public static readonly int MAIN_COLOR_ID = Shader.PropertyToID("_Color");
    public static readonly int EMISSION_MAP_ID = Shader.PropertyToID("_EmissionMap");
    public static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
    public static readonly int MAIN_TEXTURE_ID = Shader.PropertyToID("_MainTex");
    [Title("Character Components")]
    [SerializeField] private Character character;
    [SerializeField] public Animator animatorShop;

    [Title("SkinnedMeshRenderer")]
    [SerializeField] private Renderer renderer;

    [Title("Skin")]
    [SerializeField] private Transform hatParentPosition;
    [SerializeField] private List<Transform> roleWeaponParent;
    [SerializeField] private List<Transform> subWeaponParent;

    private MeshRenderer meshHat;
    private MeshRenderer meshFov;
    private SkinnedMeshRenderer mesSkill;

    private int idHat;
    private Skin skin;
    
    public DataTextureSkin DataTextureSkin => GameManager.Instance.dataTextureSkin;
    public Color SkinColor => DataTextureSkin.dictColorSkin[skin];

    public Renderer Renderer => renderer;

    public MeshRenderer MeshHat => meshHat;
    public SkinnedMeshRenderer MeshSkill => mesSkill;

    public Skill Skill { get; set; }

    public Skin Skin => skin;


    MaterialPropertyBlock blockMaterialFovRenderer;
    MaterialPropertyBlock blockMaterialBodyRenderer;

    private void Awake()
    {
        if (!character)
            return;

        if (character.fovMeshFilter)
        {
            meshFov = character.fovMeshFilter.GetComponent<MeshRenderer>();
        }

        blockMaterialFovRenderer = new MaterialPropertyBlock();
        blockMaterialBodyRenderer = new MaterialPropertyBlock();
    }

    public void Init()
    {
        InitHat();
        InitSkin();
    }

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
            idSkin = (int)Skin.SKIN_ROB;
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
        }
    }

    private void ChangeSkin(int id)
    {
        skin = (Skin)id;

        if (blockMaterialBodyRenderer == null)
            blockMaterialBodyRenderer = new MaterialPropertyBlock();

        if (renderer)
        {
            renderer.GetPropertyBlock(blockMaterialBodyRenderer);
            blockMaterialBodyRenderer.SetTexture(MAIN_TEXTURE_ID, DataTextureSkin.dictTextureSkin[(Skin)id]);
            blockMaterialBodyRenderer.SetColor(MAIN_COLOR_ID, Color.white);
            blockMaterialBodyRenderer.SetColor(EMISSION_COLOR, Color.HSVToRGB(0, 0, 0.7f));
            renderer.SetPropertyBlock(blockMaterialBodyRenderer);
        }

        if (meshFov)
        {
            if (blockMaterialFovRenderer == null)
                blockMaterialFovRenderer = new MaterialPropertyBlock();

            meshFov.GetPropertyBlock(blockMaterialFovRenderer);
            blockMaterialFovRenderer.SetColor(EMISSION_COLOR, SkinColor);
            meshFov.SetPropertyBlock(blockMaterialFovRenderer);
        }

    }

    private void ChangeHat(int id)
    {
        if (meshHat)
            Destroy(meshHat.gameObject);

        if (hatParentPosition == null)
            return;

        idHat = id;
        var hat = id >= 0 ? PrefabStorage.Instance.Hats[id] : PrefabStorage.Instance.DefaultHat;
        if (hat == null) return;
        hat = Instantiate(hat, hatParentPosition);
        meshHat = hat.GetComponent<MeshRenderer>();
    }
}
