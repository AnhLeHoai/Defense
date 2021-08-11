using UnityEngine;
using System.Collections;
using System;
using Common.FSM;
using UnityEngine.AI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx.Operators;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    public const int TotalIdlePoses = 7;
    public const int TotalVictoryPoses = 7;    
    
    [Header("Components")]
    [SerializeField] private Animator aliveAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SkinCharacterController skinCharacter;
    [SerializeField] public MeshFilter fovMeshFilter;

    [Header("Properties")]
    [SerializeField] private float speed = 7f;
    
    private PetController petController;

    private bool isAttacking;
    private bool canMove;
    private bool canKill = true;

    public float Speed => speed;

    public SkinCharacterController SkinCharacter => skinCharacter;

    public void Renew(bool isResetCollector = true)
    {
    }

    public void InitPet()
    {
        int id = GameManager.Instance.PlayerDataManager.GetIdEquipSkin(TypeEquipment.PET);

        if ((petController && petController.Id != id) || (id != -1 && !petController))
        {
            if (petController)
            {
                Destroy(petController.gameObject);
            }
            var pet = Resources.Load<PetController>("Pets/" + (TypePet)id);
            if (pet)
            {
                petController = Instantiate(pet);
                petController.Id = id;
            }
        }

        if (petController)
            petController.Init(this);

        if (petController)
        {
            petController.gameObject.SetActive(gameObject.activeInHierarchy);
        }
    }

    public void DoRoleAction()
    {
    }
}
