using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plane : Bullets
{
    [SerializeField]
    private Vector3 flyDir;
    [SerializeField]
    private float flyTime;
    [SerializeField]
    private float timeToExplode;
    [SerializeField]
    private float planeSpeed;
    [SerializeField]
    protected int maxBullets;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    private GameObject bomb;
    [SerializeField]
    private bool isActivateBomb;

    //[SerializeField]
    //protected TextMeshProUGUI countText;

    protected int currentNumberBullets;
    protected float currentReloadTime;
    private float currentFlyTime;


    public float FlyTime { get => flyTime; set => flyTime = value; }
    public float CurrentFlyTime { get => currentFlyTime; set => currentFlyTime = value; }
    public Vector3 FlyDir { get => flyDir; set => flyDir = value; }

    public int MaxBullets { get => maxBullets; set => maxBullets = value; }
    public int CurrentNumberBullets { get => currentNumberBullets; set => currentNumberBullets = value; }
    //public TextMeshProUGUI CountText { get => countText; set => countText = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public float TimeToExplode { get => timeToExplode; set => timeToExplode = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentNumberBullets = maxBullets;
        currentFlyTime = 0;
        flyDir = new Vector3(0,0.1f,1);
        currentReloadTime = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        Flying();
    }

    private void Flying()
    {
        transform.position += flyDir * planeSpeed * Time.deltaTime;
        currentFlyTime += Time.deltaTime;
            if (currentFlyTime >= timeToExplode)
            {
                if (currentNumberBullets > 0)
                {
                    currentFlyTime = 0;
                    DropBomb();
                }
                else
                {
                    if (currentReloadTime <= 0)
                    {
                        currentReloadTime = reloadTime;
                        CurrentNumberBullets = MaxBullets;
                    }
                    else
                    {
                        currentReloadTime -= Time.deltaTime;
                    }
                }
            }
            if (currentFlyTime >= flyTime)
            {
                DespawnPlane();
            }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
    protected void DropBomb()
    {
        GameObject planeBomb = SimplePool.Spawn(bomb, transform.position, Quaternion.identity);
        planeBomb.GetComponent<Bomb>().SetUp(Vector3.down);
        currentNumberBullets--;
    }

    private void DespawnPlane()
    {
        currentFlyTime = 0;
        SimplePool.Despawn(gameObject);
    }
}
