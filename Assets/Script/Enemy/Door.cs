using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animation;
    private bool active = false;

    public bool Active { get => active; set => active = value; }

    // Start is called before the first frame update
    void Awake()
    {
        animation = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.localScale.y >= 150000)
        {
            Active = true;
        }
    }
    public void ScaleDoor()
    {
        animation.SetBool("IsScale", true);
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if ( other.CompareTag("Ground"))
    //    {
    //        animation.SetBool("IsScale", false);
    //        Active = true;
    //    }
    //}

    public void BackInitialState()
    {
        animation.SetBool("BackInitial", true);
        Active = false;
    }
}
