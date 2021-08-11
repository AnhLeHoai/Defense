using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : Enemy
{
    [SerializeField]
    private float timeToChangeDir;

    private float currentMoveTime;
    public float TimeToChangeDir { get => timeToChangeDir; set => timeToChangeDir = value; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        isHeartBarActive = false;
        currentMoveTime = 0;
        moveDir = new Vector3(Random.Range(-0.5f, 0.5f), 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTouch() )
        {
            Move();
            ChangeDir();
        }
        else
        {
            animator.SetTrigger("Attack");
            animator.SetBool("Run", false);
        }
        DespawnEnemy();
    }

    protected void ChangeDir()
    {
        if ( currentMoveTime >= timeToChangeDir)
        {
            moveDir = new Vector3(Random.Range(-0.5f, 0.5f), 0, -1);
        }
        else
        {
            currentMoveTime += Time.deltaTime;
        }
    }

    public override void TakeDamage(float damage, DieType dieType, Vector3 hitPosition)
    {
        isDie = true;
        Kill.numberKills += 1;
        if (dieType != DieType.FlameGun)
        {
            animator.SetBool("Fall", true);
            StartCoroutine(WaitTimeDie());
        }
        else
        {
            Renew();
            SimplePool.Despawn(gameObject);
        }
    }

    IEnumerator WaitTimeDie()
    {
        yield return new WaitForSeconds(1);
        Renew();
        SimplePool.Despawn(gameObject);
    }

    public override void Renew()
    {
        base.Renew();
        currentMoveTime = 0;
        animator.SetBool("Fall", false);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
