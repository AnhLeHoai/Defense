using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    [SerializeField]
    GameObject fxBurst;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        bloodPrefabs.SetActive(false);
        moveDir = Vector3.back;
        isHeartBarActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDie)
        {
            Move();
        }
        DespawnEnemy();
    }

    public override void TakeDamage(float damage, DieType dieType, Vector3 hitPosition)
    {
        bloodPrefabs.SetActive(true);
        isDie = true;
        Kill.numberKills += 1;
        if (dieType != DieType.FlameGun)
        {
            animator.SetBool("Fall", true);
            StartCoroutine(WaitTimeDie());
        }
        else
        {
            fxBurst.SetActive(true);
            Renew();
            SimplePool.Despawn(gameObject);
        }
    }

    public override void Renew()
    {
        base.Renew();
        fxBurst.SetActive(false);
        bloodPrefabs.SetActive(false);
        animator.SetBool("Fall", false);
    }

}
