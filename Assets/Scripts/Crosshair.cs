using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    private Rect crossRect;
    private Texture crossTexture;
	// Use this for initialization
	void Start ()
	{
	    float crosshairSize = Screen.width*0.1f;
        crossTexture = Resources.Load("Textures/crosshair") as Texture;
        crossRect = new Rect (Screen.width/2 -8, Screen.height/2 - 8,
            crosshairSize/8,crosshairSize/8);
	}
    void OnGUI()
    {
        GUI.DrawTexture(crossRect,crossTexture);
    }

}
