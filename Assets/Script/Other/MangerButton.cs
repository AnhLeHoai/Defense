using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MangerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GunChangeButton()
    {
        GameObject tmpBtn = EventSystem.current.currentSelectedGameObject;
        int tmpBtnIndex = tmpBtn.transform.GetSiblingIndex();

        Debug.Log(tmpBtnIndex);
        GameManager.Instance.CurrentLevelManager.Player.ChangeGun(tmpBtnIndex);

    }
}
