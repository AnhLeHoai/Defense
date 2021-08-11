using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Heart : GenericSingletonClass<Heart>
{
    public static float heartRate = 100;
    TextMeshProUGUI heartText;

    // Start is called before the first frame update
    void Start()
    {
        heartText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        heartText.text = "" + (int)Mathf.Round(heartRate);
        EndGame();
    }

    void EndGame()
    {
        if (Heart.heartRate <= 0)
        {
            GameManager.Instance.CurrentLevelManager.CanavasController.LoseCanavasGame();
            GameManager.Instance.CurrentLevelManager.EndGame(LevelResult.Lose);
        }
    }
}
