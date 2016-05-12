using System;
using UnityEngine;
using System.Collections;
using UnityEngineInternal;


public class ShowThePopUp : MonoBehaviour {


    private bool _visible;
    private float _timeRemaining = 5.0f;

    int _interval = 1;

    void Start()
    {
        _visible = false;
        PopUpTimerController.Initialize();
    }

    void Update()
    {
        if (_visible)
        {
            _timeRemaining -= Time.deltaTime;
            if (Time.time >= _interval)
            {
                _interval = Mathf.FloorToInt(Time.time) + 1;
                UpdateEverySecond();
            }
        } 
    }

    void UpdateEverySecond()
    {
        if (_timeRemaining > 0)
        {
            PopUpTimerController.CreateFloatingText(Mathf.Round(_timeRemaining).ToString(), transform);
        }
    }

  void OnTriggerEnter()
    {
         _visible = true;
    }

}
