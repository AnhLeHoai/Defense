using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canavas : MonoBehaviour
{
    [SerializeField]
    GameObject information;
    [SerializeField]
    GameObject price;
    [SerializeField]
    GameObject gun;

    public void StartCanavaGame()
    {
        information.SetActive(true);
        RemainingTime.remainingTime = 30;
        Kill.numberKills = 0;
        gun.SetActive(true);
        GunButtonManager.Activate = true;
        price.SetActive(false);
    }

    public void EndCanavasGame()
    {
        GameManager.Instance.CurrentLevelManager.Player.gameObject.SetActive(false);
        ManagerEnemy.Active = false;
        Round.roundIndex += 1;
        information.SetActive(false);
        gun.SetActive(false);
        price.SetActive(true);
    }

    public void LoseCanavasGame()
    {
        GameManager.Instance.CurrentLevelManager.Player.gameObject.SetActive(false);
        ManagerEnemy.Active = false;
        RemainingTime.remainingTime = 0;
        information.SetActive(false);
        gun.SetActive(false);
        price.SetActive(false);
    }
}
