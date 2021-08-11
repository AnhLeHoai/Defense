using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        EndGame();
    }

    public static void TakeDamage(float damage)
    {
            Heart.heartRate -= damage;
    }

    void EndGame()
    {
        if (Heart.heartRate <= 0)
        {
            //GameController.Instance.EndLevelWithLose();
        }
    }
}
