using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Kill : GenericSingletonClass<Kill>
{
    public static int numberKills = -1;
    TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        numberKills = -1;
        score = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "" + ( numberKills + 1);
        if (numberKills + 1 >= ManagerEnemy.total)
        {
            StartCoroutine(WaitTimeWin());
        }   
    }


    IEnumerator WaitTimeWin()
    {
        yield return new WaitForSeconds(1);
        ListPrices.Activate = true;
        GameManager.Instance.CurrentLevelManager.CanavasController.EndCanavasGame();
        GameManager.Instance.CurrentLevelManager.EndGame(LevelResult.Win);
    }
}
