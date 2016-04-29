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
        }
}
