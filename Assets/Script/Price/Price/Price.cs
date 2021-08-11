using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class Price : MonoBehaviour, IUpdatePrice
{
    private GameObject gun;
    [SerializeField]
    private bool activate = false;

    public GameObject Gun { get => gun; set => gun = value; }
    public bool Activate { get => activate; set => activate = value; }

    public virtual void UpdatePrice()
    {

    }
    //find Gun + operation
    public abstract void Operation();

    protected GameObject FindAnActivateGun()
    {
        int tmp;
        tmp = Random.Range(0, GameManager.Instance.CurrentLevelManager.Player.listGunActive.Count - 1);
        tmp = GameManager.Instance.CurrentLevelManager.Player.listGunActive[tmp];
        return GameManager.Instance.CurrentLevelManager.Player.ListGuns[tmp];
    }

    protected int FindADeactivateGun()
    {
        int tmp;
        List<int> index = new List<int>();
        index.Add(0);
        index.Add(3);
        index.Add(5);
        GameObject gun;
        do
        {
            //tmp = Random.Range(0, GameManager.Instance.CurrentLevelManager.Player.ListGuns.Length - 1);
            //gun = GameManager.Instance.CurrentLevelManager.Player.ListGuns[tmp];

            tmp = index[Random.Range(0, index.Count)];
            gun = GameManager.Instance.CurrentLevelManager.Player.ListGuns[tmp];
        } while (GameManager.Instance.CurrentLevelManager.Player.listGunActive.Contains(tmp));

        return tmp;
    }
}
