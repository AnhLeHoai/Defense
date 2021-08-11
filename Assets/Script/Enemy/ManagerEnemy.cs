using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerEnemy : MonoBehaviour
{
    [SerializeField]
    public int enenmyNumber;
    [SerializeField]
    public int bossNumber;
    public static int total = 0;

    [SerializeField]
    public int stepTime;

    [SerializeField]
    public int moreBoss = 1;

    private static bool active = false;
    private static bool firstTimeActive = true;
    private int fixedCount = 150;
    private int enemyTotal;
    private int more;

    [SerializeField]
    private GameObject enemyType1;
    [SerializeField]
    private GameObject enemyType2;
    [SerializeField]
    private GameObject enemyType3;
    [SerializeField]
    private GameObject enemyType4;
    [SerializeField]
    private GameObject enemyType5;

    Vector3[] triangle = new Vector3[3];
    [SerializeField]
    private Transform vertex1;
    [SerializeField]
    private Transform vertex2;
    [SerializeField]
    private Transform vertex3;

    Vector3[] rectangle = new Vector3[3];
    [SerializeField]
    private Transform rectangleVertex1;
    [SerializeField]
    private Transform rectangleVertex2;
    [SerializeField]
    private Transform rectangleVertex3;

    //Door
    //[SerializeField]
    //private GameObject door;
    //private Door doorController;
    //private Transform doorPosition;

    private int success = 0;
    private int remain = 0;
    private Vector3[] enemyTypeRange = new Vector3[5];

    public static bool Active { get => active; set => active = value; }
    public static bool FirstTimeActive { get => firstTimeActive; set => firstTimeActive = value; }

    void Start()
    {
        triangle[0] = vertex1.position;
        triangle[1] = vertex2.position;
        triangle[2] = vertex3.position;

        rectangle[0] = rectangleVertex1.position;
        rectangle[1] = rectangleVertex2.position;
        rectangle[2] = rectangleVertex3.position;

        Active = false;
        //doorPosition = door.transform;
        //doorController = door.GetComponent<Door>();
        enemyTypeRange[0] = enemyType1.GetComponent<BoxCollider>().size/ 2 * enemyType1.transform.localScale.x;
        enemyTypeRange[1] = enemyType2.GetComponent<BoxCollider>().size / 2 * enemyType2.transform.localScale.x;
        enemyTypeRange[2] = enemyType3.GetComponent<BoxCollider>().size / 2 * enemyType3.transform.localScale.x;
        enemyTypeRange[3] = enemyType4.GetComponent<BoxCollider>().size / 2 * enemyType4.transform.localScale.x;
        enemyTypeRange[4] = enemyType5.GetComponent<BoxCollider>().size / 2 * enemyType5.transform.localScale.x;

    }

    private void Update()
    {
        if (Active)
        {
            //door.gameObject.SetActive(true);
            //if ( doorController.Active)
            //{
            if (FirstTimeActive)
            {
                success = SpawEnemy.SpawnEnemies(triangle, enemyType1, fixedCount, enemyTypeRange[0]);
                SpawnBoss();
                total = success;
                remain = fixedCount - success;
                FirstTimeActive = false;
            }
            else
            {
                more = Kill.numberKills - more + remain;
                success = SpawEnemy.SpawnEnemies(triangle, enemyType1, more, enemyTypeRange[0]);
                total += success;
                remain = more - success;
                more = Kill.numberKills;
                if (total >= enenmyNumber || RemainingTime.remainingTime <= 5)
                {
                    active = false;
                    //doorController.BackInitialState();
                }
            }  
        }
            //else
            //{
            //    //doorController.ScaleDoor();
            //}
        //}
        //else
        //{
        //    door.gameObject.SetActive(false);
        //}
    }

    public void SpawnBoss()
    {
        int round = Round.roundIndex;
        switch (round)
        {
            case 0:
                break;
            case 1:
                total += bossNumber;
                SpawEnemy.SpawBosses(rectangle, enemyType2, bossNumber, enemyTypeRange[1]);
                break;
            case 2:
                total += bossNumber * 2;
                SpawEnemy.SpawBosses( rectangle, enemyType2, bossNumber, enemyTypeRange[1]);
                SpawEnemy.SpawBosses(rectangle, enemyType3, bossNumber, enemyTypeRange[2]);
                break;
            case 3:
                total += bossNumber * 2;
                SpawEnemy.SpawBosses(rectangle, enemyType2, bossNumber, enemyTypeRange[1]);
                SpawEnemy.SpawBosses(rectangle, enemyType3, bossNumber, enemyTypeRange[2]);
                break;
            case 4:
                total += bossNumber;
                SpawEnemy.SpawBosses(rectangle, enemyType4, bossNumber, enemyTypeRange[3]);
                break;
            case 5:
                total += bossNumber * 3;
                SpawEnemy.SpawBosses(rectangle, enemyType2, bossNumber, enemyTypeRange[1]);
                SpawEnemy.SpawBosses(rectangle, enemyType3, bossNumber, enemyTypeRange[2]);
                SpawEnemy.SpawBosses(rectangle, enemyType4, bossNumber, enemyTypeRange[3]);
                break;
            case 6:
                total += bossNumber;
                SpawEnemy.SpawBosses(rectangle, enemyType5, bossNumber, enemyTypeRange[4]);
                break;
            case 7:
                total += bossNumber;
                SpawEnemy.SpawBosses(rectangle, enemyType3, bossNumber, enemyTypeRange[2]);
                break;
            case 8:
                total += bossNumber;
                SpawEnemy.SpawBosses(rectangle, enemyType4, bossNumber, enemyTypeRange[3]);
                break;
            default:
                break;
        }
    }


}
