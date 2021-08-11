using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemainingTime : MonoBehaviour
{
    public static float remainingTime = 30;
    TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Awake()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {    
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timeText.text = "" + Mathf.Round(remainingTime);
        }
        else
        {
            timeText.text = "Time has run out!";

            if (Heart.heartRate > 0)
            {
                ListPrices.Activate = true;
                GameManager.Instance.CurrentLevelManager.CanavasController.EndCanavasGame();
                GameManager.Instance.CurrentLevelManager.EndGame(LevelResult.Win);
            }
        }    
    }
}
