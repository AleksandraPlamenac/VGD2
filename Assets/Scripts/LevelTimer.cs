using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    private float timeRemaining = 300;
    private bool isTimeCountingDown = true;
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isTimeCountingDown)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                text.text = GetLevelTimeLeft();
            }
            else
            {
                Debug.Log("Time ran out, level passed TODO add inbetween lv scene");
                isTimeCountingDown = false;
            } 
        }
    }

    public string GetLevelTimeLeft()
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);  
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        return minutes + ":" + seconds;
    }
}
