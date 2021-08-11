using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullets
{
    [SerializeField]
    public GameObject explosionEffect;

    // Start is called before the first frame update
    private void Start()
    {
        existTime = liveTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * bulletSpeed * Time.deltaTime;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Ground"))
        {
            Explode();
        }
    }

    protected override void Explode()
    {
        SimplePool.Spawn(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider nearbyObject in colliders)
        {
            if (!nearbyObject.CompareTag("Ground") && !nearbyObject.CompareTag("Wall"))
            {
                ITakeDamage takeDamage = nearbyObject.GetComponent<ITakeDamage>();
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(damage, DieType.Bomb, transform.position);
                }
            }
        }
        SimplePool.Despawn(gameObject);
    }
}
