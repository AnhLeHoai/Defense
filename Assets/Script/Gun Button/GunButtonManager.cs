using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunButtonManager : GenericSingletonClass<GunButtonManager>
{
    [SerializeField]
    private GameObject[] gunButtons;

    private static bool activate = false;

    public static bool Activate { get => activate; set => activate = value; }

    private void Start()
    {
        for (int i = 0; i < gunButtons.Length; i++)
        {
            gunButtons[i].SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            for (int i = 0; i < GameManager.Instance.CurrentLevelManager.Player.listGunActive.Count; i++)
            {
                gunButtons[i].SetActive(true);
                gunButtons[i].GetComponent<GunButtons>().SetGun(GameManager.Instance.CurrentLevelManager.Player.listGunActive[i]);
            }
        }       
    }

    public void ChangeGun()
    {
        GameObject tmpBtn = EventSystem.current.currentSelectedGameObject;
        GameManager.Instance.CurrentLevelManager.Player.ChangeGun(GameManager.Instance.CurrentLevelManager.Player.listGunActive[tmpBtn.GetComponent<GunButtons>().Index]);
    }

    public void ResetGunButton()
    {
        for (int i = 0; i < gunButtons.Length; i++)
        {
            gunButtons[i].GetComponent<GunButtons>().ResetGun();
            gunButtons[i].SetActive(false);
        }
    }
}
