using System;
using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class ShowThePopUp : MonoBehaviour {


    private bool visible = false;
    private float timeRemaining = 5.0f;
    int interval = 1;

    void Start()
    {
        visible = false;
        PopUpTimerController.Initialize();
    }

    void Update()
    {
        if (visible)
        {
            timeRemaining -= Time.deltaTime;
            if (Time.time >= interval)
            {
                interval = Mathf.FloorToInt(Time.time) + 1;
                UpdateEverySecond();
            }
        }
    }

    void UpdateEverySecond()
    {
        if (timeRemaining > 0)
        PopUpTimerController.CreateFloatingText(Mathf.Round(timeRemaining).ToString(), transform);
    }

  void OnTriggerEnter()
    {
         visible = true;
        Debug.Log("Lopas iejo"); 
    }

    void OnTriggerExit()
    {
        visible = false;
        Debug.Log("Lopas");
    }
}
