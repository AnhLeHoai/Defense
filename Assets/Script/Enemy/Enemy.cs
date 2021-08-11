using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage, ITakeFlameDamage
{
    [SerializeField]
    public Animator animator;

    [SerializeField]
    protected GameObject bloodPrefabs;

    [SerializeField]
    public Transform characterTransform;
    public bool isDie;
    [SerializeField]
    private GameObject[] diePrebas;
    protected Vector3 moveDir;

    [SerializeField]
    protected float targetSpeed;
    [SerializeField]
    protected float targetDamage;
    [SerializeField]
    protected int blood;
    [SerializeField]
    protected HeartBar heartBar;
    [SerializeField]
    protected bool isHeartBarActive;
    [SerializeField]
    protected SkinnedMeshRenderer meshList;
    [SerializeField]
    protected Material flameMaterial;
    [SerializeField]
    protected Material initMaterial;
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public float TargetSpeed { get => targetSpeed; set => targetSpeed = value; }
    public float TargetDamage { get => targetDamage; set => targetDamage = value; }
    protected int Blood { get => blood; set => blood = value; }
    protected HeartBar HeartBar { get => heartBar; set => heartBar = value; }

    private int maxBlood;
    public virtual void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        isDie = false;
        maxBlood = blood;
    }

    protected void Move()
    {
        transform.position += moveDir * targetSpeed * Time.deltaTime;
    }

    protected void DespawnEnemy()
    {
        if (RemainingTime.remainingTime <= 0)
        {
            Renew();
            SimplePool.Despawn(gameObject);
        }
    }

    public GameObject Die(DieType dieType)
    {
        GameObject die = null;
        switch (dieType)
        {
            case DieType.BurstGun:
                //die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Bomb:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.FlameGun:
                //die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Cannon:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Laser:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
        if ( die != null)
        {
            die.transform.localScale = transform.localScale / 2;

        }
        return die;
    }
    public virtual void TakeDamage(float damage, DieType dieType, Vector3 hitPosition)
    {
        if ( isHeartBarActive == true)
        {
            Blood -= (int)damage;
            if (Blood > 0)
            {
                GameObject blood = SimplePool.Spawn(bloodPrefabs, hitPosition, Quaternion.identity);
                StartCoroutine(WaitTimeDespawnBlood(blood));
                HeartBar.SetHealth(Blood);
            }
            else
            {
                animator.SetBool("Fall", true);
                StartCoroutine(WaitTimeDie());
                isDie = true;
                Kill.numberKills += 1;
                GameObject die = Die(dieType);
                if ( die != null)
                {
                    die.GetComponent<DiePoser>().Initialize(hitPosition);
                }
            }
        }
        //else
        //{
        //    bloodPrefabs.SetActive(true);
        //    isDie = true;
        //    Kill.numberKills += 1;
        //    if (dieType != DieType.FlameGun)
        //    {
        //        animator.SetBool("Fall", true);
        //        StartCoroutine(WaitTimeDie());
        //    }
        //    else
        //    {
        //        Renew();
        //        SimplePool.Despawn(gameObject);
        //    }
        //}
    }

    public virtual void TakeFlameDamage( float damage, float timeExist, DieType dieType, Vector3 hitPosition)
    {
        if (timeExist > 0)
        {
            Vector3 moveDir2 = new Vector3(Random.Range(-1, 1), 0, -1);
            moveDir = moveDir2;
            try
            {
                meshList.material = flameMaterial;
                targetSpeed += 0.02f;
            }
            catch (System.Exception)
            {
                Debug.LogError("Enemy doesn't have renderer");
            }
            StartCoroutine(WaitTimeBurn(damage, timeExist, dieType, hitPosition));
        }
        else
        {
            moveDir = Vector3.back;
            meshList.material = initMaterial;
            targetSpeed -= 0.02f;
            TakeDamage(damage, dieType, hitPosition);
        }
    }

    IEnumerator WaitTimeBurn(float damage, float timeExist, DieType dieType, Vector3 hitPosition)
    {
        yield return new WaitForSeconds(timeExist);
        TakeFlameDamage(damage, 0, dieType, hitPosition);
    }

    IEnumerator WaitTimeDespawnBlood( GameObject blood)
    {
        yield return new WaitForSeconds(1);
        SimplePool.Despawn(blood);
    }

    public IEnumerator WaitTimeDie()
    {
        yield return new WaitForSeconds(1);
        Renew();
        SimplePool.Despawn(gameObject);
    } 
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Wall.TakeDamage(targetDamage);
            Renew();
            SimplePool.Despawn(gameObject);
        }
    }

    protected bool IsClose()
    {
        float dis = Vector3.Distance(transform.position, Player.Cam.transform.position);
        return dis <= 25;
    }

    protected bool IsTouch()
    {
        float dis = Vector3.Distance(transform.position, Player.Cam.transform.position);
        return dis <= 10;
    }

    public virtual void Renew()
    {
        animator.SetBool("Fall", false);
        isDie = false;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        characterTransform.rotation = Quaternion.Euler(new Vector3 (0, 0,0));
        blood = maxBlood;
        moveDir = Vector3.back;
        meshList.material = initMaterial;
    }
}
