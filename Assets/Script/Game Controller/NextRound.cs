using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRound : MonoBehaviour
{
    public void GoNextRound()
    {
        //GameController.Instance.ResetLevel();
        SceneManager.LoadScene("Level1");
    }
}
