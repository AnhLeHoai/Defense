using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    protected Vector3 shootDir;

    [SerializeField]
    protected float bulletSpeed;
    [SerializeField]
    protected float liveTime;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected float range;

    protected float existTime;

    public float Damage { get => damage; set => damage = value; }
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
    public Vector3 ShootDir { get => shootDir; set => shootDir = value; }
    public float LiveTime { get => liveTime; set => liveTime = value; }
    public float ExistTime { get => existTime; set => existTime = value; }
    public float Range { get => range; set => range = value; }

    // Start is called before the first frame update

    public void SetUp(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        existTime = liveTime;
    }

    // Update is called once per frame
    protected virtual void Operation()
    {
        transform.position += shootDir * bulletSpeed * Time.deltaTime;
        existTime -= Time.deltaTime;
        if (existTime <= 0)
        {
            DespawnBullet();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if ( other.GetComponent<Enemy>() != null)
        {
            if (!other.GetComponent<Enemy>().isDie)
            {
                Explode();
            }
        }
    }

    protected virtual void Explode()
    {
    }


    protected virtual void CauseDamage(Collider other, DieType dieType)
    {
        Vector3 hitPosition = other.ClosestPoint(transform.position);
        ITakeDamage takeDamage = other.GetComponent<ITakeDamage>();

        if (takeDamage != null)
        {
            takeDamage.TakeDamage(damage, dieType, hitPosition);
        }
        DespawnBullet();

    }

    protected void DespawnBullet()
    {
        existTime = liveTime;
        SimplePool.Despawn(gameObject);
    }
}
