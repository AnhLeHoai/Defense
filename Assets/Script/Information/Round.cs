using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Round : MonoBehaviour
{
    public static int roundIndex = 0;
    TextMeshProUGUI roundText;
    // Start is called before the first frame update
    void Start()
    {
        roundText = GetComponent<TextMeshProUGUI>();
        roundText.text = "Round: " + roundIndex;
    }
}
