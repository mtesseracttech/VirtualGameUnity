using UnityEngine;
using System.Collections;

public class PlayerControlls : MonoBehaviour {
    public Rigidbody rb;
    private int ForwardSpeed = 0;
    private int SidewaysSpeed = 0;
    private int UpwardsSpeed = 0;

    ////////////// Variables used for player and camera rotation ///////
    public Transform camera;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;
    ////////////////////////////////////////////////////////////////////
    void Update()
    {
        PlayerMovement();
        PlayerAndCameraRotation();
    }

    void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }
    void PlayerMovement()
    {
        Vector3 _velocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpwardsSpeed = 10;
        }
        else
        {
            UpwardsSpeed = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            ForwardSpeed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ForwardSpeed = -1;
        }
        else
        {
            ForwardSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            SidewaysSpeed = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SidewaysSpeed = 1;
        }
        else
        {
            SidewaysSpeed = 0;
        }
        Vector3 direction = new Vector3(SidewaysSpeed, UpwardsSpeed, ForwardSpeed);
        if (rb.velocity.magnitude <= 3)
        {
            rb.AddRelativeForce(direction * 10);
        }
        //Debug.Log("Walk: " + _velocity.magnitude);
    }
    void PlayerAndCameraRotation()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            camera.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
    }
}
