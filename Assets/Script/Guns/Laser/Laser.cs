using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullets
{
    [SerializeField]
    public GameObject explosionEffect;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (!other.GetComponent<Enemy>().isDie)
            {
                Vector3 hitPosition = other.ClosestPoint(transform.position);
                ExplodeLaser(hitPosition);
            }
        }

    }
    void ExplodeLaser(Vector3 hitPosition)
    {
        Instantiate(explosionEffect, hitPosition, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(hitPosition, range);
        foreach (Collider nearbyObject in colliders)
        {
            if (!nearbyObject.CompareTag("Ground") && !nearbyObject.CompareTag("Wall"))
            {
                ITakeDamage takeDamage = nearbyObject.GetComponent<ITakeDamage>();
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(damage, DieType.Laser, transform.position);
                }
            }
        }
    }
}
