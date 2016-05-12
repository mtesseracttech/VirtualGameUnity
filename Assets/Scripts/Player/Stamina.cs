using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour
{
    private float stamina = 5;
    private float maxStamina = 5;
    private Rect staminaRect;
    private Texture2D staminaTexture;
    private bool isRunning;
    private bool staminaOut;
    private Tackle tackle;
    float walkSpeed, runSpeed;

    void Start()
    {
        tackle = gameObject.GetComponent<Tackle>();
        staminaRect = new Rect(Screen.width/10, Screen.height*9/10,Screen.width/3,Screen.height/50);
        staminaTexture = new Texture2D(1,1);
        staminaTexture.SetPixel(0,0,Color.green);
        staminaTexture.Apply();
    }
    void Update()
    {
        if (tackle.TackleBool || (Input.GetKey(KeyCode.LeftShift) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))))
            isRunning = true;
        else
            isRunning = false;

        if (isRunning)
        {
            tackle.TackleBool = true;
            stamina -= Time.deltaTime / 2;
            staminaOut = false;

            if (stamina < 0)
            {
                stamina = 0;
                tackle.TackleBool = false;
                staminaOut = true;
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += Time.deltaTime / 10;
            tackle.TackleBool = false;
        }
    }

    void OnGUI()
    {
        float ratio = stamina/maxStamina;
        float rectWidth =ratio* Screen.width/3;
        staminaRect.width = rectWidth;
        GUI.DrawTexture(staminaRect,staminaTexture);
    }

    public bool StaminaCheck(bool staminaEnergy)
    {
        staminaEnergy = staminaOut;
        return staminaEnergy;
    }

}
