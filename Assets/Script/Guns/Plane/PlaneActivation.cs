using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneActivation : Guns
{
    // Start is called before the first frame update
    void Start()
    {
        CurrentReloadTime = reloadTime;
        CurrentNumberBullets = maxBullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNumberBullets > 0)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 100f))
                {
                    if (raycastHit.transform != null)
                    {
                        Debug.Log(raycastHit.transform.gameObject.tag);
                        if (raycastHit.transform.gameObject.tag == "PlaneActivation")
                        {
                            Debug.Log("hit");
                            GameObject plane = SimplePool.Spawn(bullet, new Vector3(0,60,5), Quaternion.Euler(0,90,0));
                            currentNumberBullets -= 1;
                        }
                    }
                }
            }
        }
        else
        {
            if (CurrentReloadTime <= 0)
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

    protected override void Shooting()
    {
        throw new System.NotImplementedException();
    }
}
