using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : Bullets
{
    // Start is called before the first frame update
    void Start()
    {
        existTime = liveTime;
    }

    // Update is called once per frame
    void Update()
    {
        Operation();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
   public void OnCollisionEnter(Collision otherCollision)
    {
        GameObject other = otherCollision.gameObject;
        if (other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDie )
        {
            ITakeDamage takeDamage = other.GetComponent<ITakeDamage>();

            if (takeDamage != null)
            {
                takeDamage.TakeDamage(damage, DieType.Cannon, transform.position);
            }
            if (!other.CompareTag("EnemyType1"))
            {
                SimplePool.Despawn(gameObject);

            }
        }
   }
}
