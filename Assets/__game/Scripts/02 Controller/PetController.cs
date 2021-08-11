using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Character owner;
    private int id = -1;
    public float radiusBehindOwner = 3f;
    private Transform transPlayer;
    private float speed = 8f;

    private Vector3 offsetLastPosition;
    private float offsetTimeDelay = 2f;
    private float offsetTimeCounter = 0f;

    public float Speed => Mathf.Max(owner.Speed, speed);

    public int Id { get => id; set => id = value; }
    public Animator Animator { get => animator; }

    private Vector3 target;

    private void Awake()
    {
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }

    public void Init(Character player)
    {
        transPlayer = player.transform;
        transform.position = transPlayer.position + GetOffsetPositionTowardOwner();
        navMeshAgent.Warp(transform.position);
        owner = player;
        animator.SetTrigger(CharacterAction.Action.ToAnimatorHashedKey());
    }

    private void Update()
    {
        if (!owner)
            return;

        if (GameManager.Instance.IsLevelLoading)
            return;
        
        target = GetOffsetPositionTowardOwner() + transPlayer.position;
        navMeshAgent.SetDestination(target);
        float distance = Vector3.Distance(transPlayer.position, transform.position);
        float speed = Speed * (distance / (radiusBehindOwner / 0.5f));
        navMeshAgent.speed = speed;
        
        Vector3 newPosition = navMeshAgent.nextPosition;

        if (newPosition != transform.position)
        {
            animator.SetBool(CharacterAction.Run.ToAnimatorHashedKey(), true);
        }
        else
        {
            animator.SetBool(CharacterAction.Run.ToAnimatorHashedKey(), false);
        }

        RotateFacing(newPosition - transform.position);
        transform.position = newPosition;
    }

    private Vector3 GetOffsetPositionTowardOwner()
    {
        if (offsetTimeCounter > 0)
        {
            offsetTimeCounter -= Time.deltaTime;
            return offsetLastPosition;
        };
        offsetTimeCounter = offsetTimeDelay;

        Vector3 behindOwnerDirection = -transPlayer.forward;
        float angleOffsetTowardOwner = VectorUlti.GetAngleFromVector(behindOwnerDirection) + Random.Range(-90, 90);
        Vector3 directionTowardOwner = VectorUlti.GetVectorFromAngle(angleOffsetTowardOwner).Set(y: 0).normalized;
        offsetLastPosition = (directionTowardOwner * radiusBehindOwner);

        return offsetLastPosition;
    }

    private void RotateFacing(Vector3 directionFacing)
    {
        if (directionFacing == Vector3.zero)
            return;

        var targetRotation = Quaternion.LookRotation(directionFacing);

        transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                10f);
    }
}
