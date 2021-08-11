using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType5 : Enemy
{
    [SerializeField]
    private GameObject bomb;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        moveDir = Vector3.back;
        isHeartBarActive = true;
        heartBar.SetMaxHealth(blood);
        //bomb.transform.SetParent(transform.parent);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DespawnEnemy();
    }

    public override void TakeDamage(float damage, DieType dieType, Vector3 hitPosition)
    {
        base.TakeDamage(damage, dieType, hitPosition);
        Blood -= (int)damage;
        if (blood > 0)
        {
            HeartBar.SetHealth(Blood);
        }
        else
        {
            GameObject enemyBomb = SimplePool.Spawn(bomb, transform.position + new Vector3(0, 100, 0), Quaternion.identity);
            enemyBomb.GetComponent<Bomb>().SetUp(Vector3.down);
            Kill.numberKills += 1;
            Renew();
            SimplePool.Despawn(gameObject);
        }
    }
}
