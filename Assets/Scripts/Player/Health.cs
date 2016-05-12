using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    private float health = 5;
    private float maxHealth = 5;
    private Rect healthRect;
    private Texture2D heaTexture2D;
    private Stamina stamina;
    private bool healthRunningOut;

    void Start()
    {
        stamina = GetComponent<Stamina>();
        healthRect = new Rect(Screen.width / 10, 900, Screen.width /3, Screen.height / 50);
        heaTexture2D = new Texture2D(1, 1);
        heaTexture2D.SetPixel(0, 0, Color.red);
        heaTexture2D.Apply();
        healthRunningOut = false;
    }

    void OnGUI()
    {
        float ratio = health / maxHealth;
        float rectWidth = ratio * Screen.width / 3;
        healthRect.width = rectWidth;
        GUI.DrawTexture(healthRect, heaTexture2D);
    }

    void Update()
    {
        if (stamina.StaminaCheck(true) && (Input.GetKey(KeyCode.LeftShift) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))))
            healthRunningOut = true;
        else healthRunningOut = false;

        if (healthRunningOut)
        {
            health -= Time.deltaTime/10;
            Debug.Log("health die");
            if (health < 0)
            {
                health = 0;
                healthRunningOut = false;
                Debug.Log("die");
            }
        }
        else if ( health < maxHealth && !healthRunningOut)
        {
            health += Time.deltaTime/2;
        }

        Debug.Log(healthRunningOut);
            
    }
}
