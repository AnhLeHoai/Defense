using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType4 : Enemy
{
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private int shieldBlood;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        moveDir = Vector3.back;
        isHeartBarActive = true;
        heartBar.SetMaxHealth(blood);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsClose())
        {
            Move();
        }
        else
        {
            animator.SetTrigger("Attack");
            animator.SetBool("Run", false);
        }

        DespawnEnemy();
    }

    public override void TakeDamage(float damage, DieType dieType, Vector3 hitPosition)
    {
        base.TakeDamage(damage, dieType, hitPosition);
        if ( shieldBlood <= 0)
        {
            blood -= (int)damage;
            //if ( blood > 0)
            //{
            //    heartBar.SetHealth(blood);
            //}
            //else
            //{
            //    Kill.numberKills += 1;
            //    Renew();
            //    SimplePool.Despawn(gameObject);
            //}
        }
        else
        {
            shieldBlood -= (int)damage;

            if ( shieldBlood <= 0)
            {
                Destroy(shield);
            }
        }
    }
}
