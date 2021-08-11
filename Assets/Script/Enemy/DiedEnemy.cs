using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiedEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject[] diePrebas;

    public GameObject Die(DieType dieType)
    {
        GameObject die = null;
        switch (dieType)
        {
            case DieType.BurstGun:
                //die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Bomb:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.FlameGun:
                //die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Cannon:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            case DieType.Laser:
                die = SimplePool.Spawn(diePrebas[0], transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
        if (die != null)
        {
            die.transform.localScale = transform.localScale / 2;

        }
        return die;
    }

    public void SpawnDiedEnemy(int count, Vector3 hitPosition, DieType dieType)
    {
        for ( int i = 0; i < count; i++)
        {
            GameObject die = Die(dieType);
            if (die != null)
            {
                die.GetComponent<DiePoser>().Initialize(hitPosition);
            }
        }
    }
}
