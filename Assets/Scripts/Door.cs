using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Light thisLight;
    private Color originalColor;
    private float timePassed;
    private float changeValue;

    void Start()
    {
        thisLight = this.GetComponent<Light>();
        if (thisLight != null)
            originalColor = thisLight.color;
        else
            {
                enabled = false;
                return;
            }
        changeValue = 0;
        timePassed = 0;
    }

    void Update()
    {
        timePassed = Time.time;
        timePassed = timePassed - Mathf.Floor(timePassed);
        thisLight.color = originalColor*CalculateChange();
    }

    private float CalculateChange()
    {
        changeValue = -Mathf.Sin(timePassed*12*Mathf.PI)*0.05f;
        return changeValue;
    }


}
