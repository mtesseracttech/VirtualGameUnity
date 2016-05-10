using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
    public PlayerControls player;
    ////////////// Variables used for camera shake effect when the player jumps /////
    private bool shake;
    private bool up;
    private bool jumped;
    public int limit;
    private int counter;
    /////////////////////////////////////////////////////////////////////////////////

    // Use this for initialization
    void Start () {
        shake = false;
        up = false;
        jumped = false;
        limit = 5;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        OnJumpCameraShake();
	}
    void OnJumpCameraShake()
    {
        if (player.Grounded() == false)
        {
            jumped = true;
        }
        if (player.Grounded() == true && jumped == true)
        {
            shake = true;
            jumped = false;
        }
        if (shake)
        {
            Vector3 stepVector = new Vector3(0, 0.01f, 0);
            if (!up && counter < limit)
            {
                gameObject.transform.position -= stepVector;
                counter += 1;
                if (counter >= limit)
                {
                    up = true;
                    counter = 0;
                }
            }
            else if (up && counter < limit)
            {
                gameObject.transform.position += stepVector;
                counter += 1;
                if (counter >= limit)
                {
                    up = false;
                    shake = false;
                    counter = 0;
                }
            }
        }
    }
}
