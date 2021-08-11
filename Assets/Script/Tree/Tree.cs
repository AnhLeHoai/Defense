using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private int blood = 200;

    public void TakeDamage(int damage)
    {
        if (blood >= 0)
        {
            blood -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
