using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFlame : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float liveTime;
    private float existTime;
    [SerializeField]
    private float timeExist;
    [SerializeField]
    private float range;

    private void Start()
    {
        existTime = liveTime;
    }
    private void Update()
    {
        existTime -= Time.deltaTime;
        if (existTime <= 0)
        {
            existTime = liveTime;
            SimplePool.Despawn(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null && !other.GetComponent<Enemy>().isDie)
        {
            Burn();
        }
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
        SimplePool.Despawn(gameObject);
    }
}
