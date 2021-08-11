using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePoser : MonoBehaviour
{
    [SerializeField]
    private Collider[] hitColliders;
    [SerializeField]
    private float hitForce;

    public void Initialize( Vector3 hitPosition )
    {
        foreach( Collider collider in hitColliders)
        {
            Vector3 attackPosition = collider.ClosestPoint(hitPosition);
            collider.attachedRigidbody.AddExplosionForce(hitForce, attackPosition, 20);
            if ( collider.CompareTag("Head"))
            {
                collider.attachedRigidbody.AddTorque(hitForce * Vector3.one);
            }
        }

        if (RemainingTime.remainingTime <= 0)
        {
            SimplePool.Despawn(gameObject);
        }
        else
        {
            StartCoroutine(WaitTimeDespawn());
        }
    }

    IEnumerator WaitTimeDespawn()
    {
        yield return new WaitForSeconds(2);
        SimplePool.Despawn(gameObject);
    }
}
