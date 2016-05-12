
using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject player;

    ////////////// Variables used for camera shake effect when the player jumps /////
    private bool shake;
    private bool jumpUp;
    private bool jumped;
    public float horizontalShakeValue;
    public float verticalViewBobbingValue;
    public int jumpLimit;
    private int jumpCounter;

    ////////////// Variables used for View Bobbing //////////////////////////////////
    private float cameraY;
    private bool equalize;
    private bool bob;
    private bool bobUp;
    private int bobLimit;
    private int bobCounter;

    // Use this for initialization
    void Start()
    {
        ////////////////
        shake = false;
        jumpUp = false;
        jumped = false;
        horizontalShakeValue = 0.075f;
        verticalViewBobbingValue = 0.0075f;
        jumpLimit = 5;
        jumpCounter = 0;

        equalize = true;
        cameraY = gameObject.transform.localPosition.y;
        bob = false;
        bobLimit = 20;
        bobCounter = 0;
        bobUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnJumpCameraShake();
        ViewBobbing();
    }
    void ViewBobbing()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            bob = true;
        Vector3 stepVector = new Vector3(0, verticalViewBobbingValue, 0);
        if (bob)
        {
            if (!bobUp)
            {
                gameObject.transform.localPosition -= stepVector;
                bobCounter += 1;
                if (bobCounter >= bobLimit)
                {
                    bobUp = !bobUp;
                    bobCounter = 0;
                }
            }
            if (bobUp)
            {
                gameObject.transform.localPosition += stepVector;
                bobCounter += 1;
                if (bobCounter >= bobLimit)
                {
                    bobUp = !bobUp;
                    bobCounter = 0;
                }
            }
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            bob = false;
            bobUp = false;
            gameObject.transform.localPosition = new Vector3(transform.localPosition.x, cameraY, transform.localPosition.z);
            bobCounter = 0;
        }
        //Debug.Log(cameraY + "" + gameObject.transform.position.y);
    }
    void OnJumpCameraShake()
    {
        if (player.GetComponent<PlayerControls2>().Grounded() == false)
        {
            jumped = true;
        }
        if (player.GetComponent<PlayerControls2>().Grounded() && jumped)
        {
            shake = true;
            jumped = false;
        }
        if (shake)
        {
            Vector3 stepVector = new Vector3(0, horizontalShakeValue, 0);
            if (!jumpUp && jumpCounter < jumpLimit)
            {
                gameObject.transform.localPosition -= stepVector;
                jumpCounter += 1;
                if (jumpCounter >= jumpLimit)
                {
                    jumpUp = true;
                    jumpCounter = 0;
                }
            }
            else if (jumpUp && jumpCounter < jumpLimit)
            {
                gameObject.transform.localPosition += stepVector;
                jumpCounter += 1;
                if (jumpCounter >= jumpLimit)
                {
                    jumpUp = false;
                    shake = false;
                    jumpCounter = 0;
                }
            }
        }
    }
}

