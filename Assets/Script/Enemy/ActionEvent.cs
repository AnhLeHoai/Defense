using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent : MonoBehaviour
{
    [SerializeField]
    public float damage;
    public void Attack()
    {
        Heart.heartRate -= damage;
    }
}
