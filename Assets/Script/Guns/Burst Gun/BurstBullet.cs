using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBullet : Bullets
{
    // Start is called before the first frame update
    void Start()
    {
        existTime = liveTime;
    }
    private void Update()
    {
        Operation();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.GetComponent<Enemy>() != null)
        {
            if (!other.GetComponent<Enemy>().isDie)
            {
                CauseDamage(other, DieType.BurstGun);
            }
        }
    }
    //protected override void Explode()
    //{
    //    base.Explode();
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.01f);
    //    foreach (Collider hit in hitColliders)
    //    {
    //        if (hit.GetComponent<Enemy>() != null && !hit.GetComponent<Enemy>().isDie)
    //        {

    //            if (hit.CompareTag("EnemyType1"))
    //            {
    //                Vector3 contactPosition = hit.ClosestPoint(transform.position);
    //                Rigidbody rb = hit.GetComponent<Rigidbody>();
    //                if (rb != null)
    //                {
    //                    rb.isKinematic = false;
    //                    rb.AddExplosionForce(5, contactPosition, range / 10, 1.0f, ForceMode.Impulse);
    //                }
    //                CauseDamage(hit, DieType.BurstGun);
    //            }
    //            else
    //            {
    //                CauseDamage(hit, DieType.BurstGun);
    //            }
    //        }
    //    }
    //}
}
