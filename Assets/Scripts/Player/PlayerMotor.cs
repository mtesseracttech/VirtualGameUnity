using UnityEngine;
using System.Collections;

//always want a rigidbody to have with playermotor
[RequireComponent(typeof (Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    //set of funcions that will make the player move, jump, fly
    //utily script that take directions velocitys and apply to rigid body
    // Use this for initialization
    private Vector3 velocity = Vector3.zero; //default
    private Vector3 rotaion = Vector3.zero;

    private float cameraRotaionX = 0f;
    private float currentCameraRotation = 0f;
    [SerializeField]
    private float _cameraRotationLimit = 40f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 pVelocity)
    {
        velocity = pVelocity;
    }
    //runs on a fixed time
    //run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
            //takes a position we want to move. 
            //rigid body if something is in the way
            //move rb to the position of velocity in certain time
            rb.MovePosition(rb.position + velocity*Time.fixedDeltaTime);
    }

    public void Rotate(Vector3 pRotation)
    {
        rotaion = pRotation;
    }

    void PerformRotation()
    {
        //quentinion system. just with imaginary component
        //quaternion takes a rotation and make it to quaternion because rb.rotation is quaternion
        rb.MoveRotation(rb.rotation  * Quaternion.Euler(rotaion));
        if (cam != null)
        {
            //set our rotation and clamp it
            currentCameraRotation -= cameraRotaionX;
            currentCameraRotation = Mathf.Clamp(currentCameraRotation, -_cameraRotationLimit, _cameraRotationLimit);
            //apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotation,0f,0f);
        }  
    }
    public void RotateCamera(float pcamRotateX)
    {
        cameraRotaionX = pcamRotateX;
    }
}
