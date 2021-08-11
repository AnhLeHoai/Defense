using TMPro;
using UnityEngine;
using System.Collections;

public abstract class Guns : MonoBehaviour
{
    [SerializeField]
    protected GameObject bullet;

    [SerializeField]
    protected int maxBullets;
    [SerializeField]
    protected float reloadTime;

    [SerializeField]
    protected GameObject shootAnim;

    protected int currentNumberBullets;
    private float currentReloadTime;

    public GameObject Bullet { get => bullet; set => bullet = value; }
    public int MaxBullets { get => maxBullets; set => maxBullets = value; }
    public int CurrentNumberBullets { get => currentNumberBullets; set => currentNumberBullets = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public float CurrentReloadTime { get => currentReloadTime; set => currentReloadTime = value; }

    protected virtual void Operation()
    {
        if (CurrentNumberBullets > 0)
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameObject shootClone = SimplePool.Spawn(shootAnim, shootAnim.transform.position, Quaternion.identity);
                Shooting();
                WaitTimeEnableAnim(shootClone);
            }
        }
        else
        {
            if ( CurrentReloadTime <= 0 )
            {
                CurrentReloadTime = reloadTime;
                CurrentNumberBullets = MaxBullets;
            }
            else
            {
                CurrentReloadTime -= Time.deltaTime;
            }
        }
    }

    public IEnumerator WaitTimeEnableAnim(GameObject shootClone)
    {
        yield return new WaitForSeconds(0.5f);
        shootClone.SetActive(false);
    }
    protected abstract void Shooting();
}
