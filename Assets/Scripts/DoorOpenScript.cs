using UnityEngine;
using System.Collections;

public class DoorOpenScript : MonoBehaviour
{
    public bool IsOpen;                 //bool for checking if door is open 
    public bool IsClosed;               //bool for checking if door is closed
    public bool InTrigger = false;              //bool for checking if trigger is triggered

    void Start()
    {
        IsOpen = false;
        IsClosed = true;
    }

    //------------------------------------------------------------------------------------------//
    //                             When enters the trigger. 
    //------------------------------------------------------------------------------------------//
    void OnTriggerEnter(Collider collider)
    {
        InTrigger = true;
    }
    //------------------------------------------------------------------------------------------//
    //                             When exits the trigger. 
    //------------------------------------------------------------------------------------------//
    void OnTriggerExit(Collider collider)
    {
        InTrigger = false;
    }
    void Update()
    {
        if (InTrigger)  //if it's in collider box
        {
            if (IsClosed)   //if door is closed
            {
                    if (Input.GetKeyDown(KeyCode.E))    //use E button
                    {
                        IsOpen = true;              //can be opened
                        IsClosed = false;
                    }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))    //use E button
                {
                    IsClosed = true;            //door is closed have a key
                    IsOpen = false;
                }
            }
        }

        if (IsOpen) //if open is true
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, -90.0f, 0.0f), // representing rotation of open door. 
                Time.deltaTime * 20);
            transform.rotation = newRot; //rotating position by using quaternion
        }
        else
        {
            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), Time.deltaTime * 20);
            transform.rotation = newRot;
        }
    }
    void OnGUI()
    {
        if (InTrigger)
        {
            if (IsOpen)
            {
                GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Press E to close");
            }
            else
            {
                if (IsClosed)
                {
                    GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Press E to open");
                }
                
            }
        }
    }

}
