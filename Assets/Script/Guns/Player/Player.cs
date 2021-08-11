using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    private static Camera cam;

    //gun
    [SerializeField]
    private GameObject[] listGuns;
    [SerializeField]
    public int currentGunActive;
    public List<int> listGunActive = new List<int>();
    [SerializeField]
    public GunButtonManager gunButtonManager;

    //foresight
    [SerializeField]
    private GameObject foresight;

    private Vector3 initPos;
    private Vector3 finPos;
    [SerializeField]
    private float rotSpeed;

    public Vector3 InitPos { get => initPos; set => initPos = value; }
    public Vector3 FinPos { get => finPos; set => finPos = value; }
    public float RotSpeed { get => rotSpeed; set => rotSpeed = value; }

    public static Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
            return cam;
        }

        set => cam = value;
    }

    public GameObject Foresight { get => foresight; set => foresight = value; }
    public GameObject[] ListGuns { get => listGuns; set => listGuns = value; }

    // Start is called before the first frame update
    void Awake()
    {
        foresight.SetActive(false);
        listGunActive.Add(0);
        GameController.Instance.Init(listGuns);
        GameController.Instance.LogInGame();

        //ResetValueAtBeginning();
    }

    // Update is called once per frame
    void Update()
    {
        Turning();
        ChangeForesight();
    }

    protected void Turning()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 rotateDir;

            var mousePos = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                InitPos = Cam.ScreenToViewportPoint(mousePos);
            }
            else
            {
                FinPos = Cam.ScreenToViewportPoint(mousePos);

                rotateDir = FinPos - InitPos;
                transform.eulerAngles += new Vector3(-rotateDir.y, rotateDir.x, 0) * RotSpeed;
                InitPos = FinPos;

                var z = transform.eulerAngles;
                if (z.y > 180) 
                     z.y -= 360;
                z.y = Mathf.Clamp(z.y, -50, 80);
                if (z.x > 180)
                {
                    z.x -= 360;
                }
                z.x = Mathf.Clamp(z.x, -45, 75);
                transform.eulerAngles = z;
            }
        }
    }

    private void ChangeForesight()
    {
        if (currentGunActive == 1 || currentGunActive == 4)
        {
            foresight.SetActive(false);
        }
        else
        {
            foresight.SetActive(true);
        }
    }

    void ActivateAGun(int index)
    {
        currentGunActive = index;
        ListGuns[currentGunActive].SetActive(true);
    }

    void DeactiveAGun(int index)
    {
        ListGuns[index].SetActive(false);
    }

    public void ChangeGun(int index)
    {
        DeactiveAGun(currentGunActive);
        ActivateAGun(index);
    }

    public void SetInitialValueOfGun()
    {
        Guns gun, thisGun;
        for (int i = 0; i < ListGuns.Length; i++)
        {
            gun = GameController.Instance.InitListGuns[i];
            thisGun = listGuns[i].GetComponent<Guns>();
            thisGun.MaxBullets = gun.MaxBullets;
            thisGun.CurrentNumberBullets = gun.MaxBullets;
            thisGun.Bullet.GetComponent<Bullets>().Range = gun.Bullet.GetComponent<Bullets>().Range;
            thisGun.ReloadTime = gun.ReloadTime;
            DeactiveAGun(i);
        }
        for (int i = 0; i < GameController.Instance.initListGunActive.Count; i++)
        {
            if (!listGunActive.Contains(GameController.Instance.initListGunActive[i]))
            {
                listGunActive.Add(GameController.Instance.initListGunActive[i]);
            }
        }
        ActivateAGun(0);
        GunButtonManager.Activate = true;
    }

    public void StartLevel()
    {
        if ( GameManager.Instance.CurrentLevel %8 == 0)
        {
            ResetValueAtBeginning();
            return;
        }
        SetInitialValueOfGun();
    }

   public void ResetValueAtBeginning()
   {
        GameController.Instance.ResetGame();
        ManagerEnemy.Active = false;
        RemainingTime.remainingTime = 30;
        Kill.numberKills = 0;
        Heart.heartRate = 100;
        gunButtonManager.ResetGunButton();
   }
}
