using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Bullets
{
    [SerializeField]
    private float timeExist;
    [SerializeField]
    private GameObject firePrefabs;

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
        if (other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDie)
        {
            Burn();
        }
        if (other.CompareTag("Ground"))
        {
            SpawnFire();
        }
        DespawnBullet();
    }

    private void Burn()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider nearbyObject in colliders)
        {
            if (!nearbyObject.CompareTag("Ground") && !nearbyObject.CompareTag("Wall"))
            {
                ITakeFlameDamage takeFlameDamage = nearbyObject.GetComponent<ITakeFlameDamage>();

                if (takeFlameDamage != null)
                {
                    takeFlameDamage.TakeFlameDamage(damage, timeExist, DieType.FlameGun, transform.position);
                }
            }
        }
    }

    private void SpawnFire()
    {   
        GameObject fire = SimplePool.Spawn(firePrefabs, transform.position, Quaternion.identity);
    }
}
