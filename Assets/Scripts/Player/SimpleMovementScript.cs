using UnityEngine;
using System.Collections;

public class SimpleMovementScript : MonoBehaviour {
    ////////////// Variables used for player movement //////////////////
    public Rigidbody rb;
    private int _forwardSpeed = 0;
    private int _sidewaysSpeed = 0;
    private int _upwardsSpeed = 0;
    ////////////////////////////////////////////////////////////////////

    ////////////// Variables used for player and camera rotation ///////
    [SerializeField]
    private Transform _camera;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float SensitivityX = 15F;
    public float SensitivityY = 15F;
    public float MinimumX = -360F;
    public float MaximumX = 360F;
    public float MinimumY = -60F;
    public float MaximumY = 60F;
    float _rotationY = 0F;


    // Use this for initialization
    void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerAndCameraRotation();
    }

    void PlayerMovement()
    {
        Vector3 _velocity = rb.velocity;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _upwardsSpeed = 10;
        }
        else
        {
            _upwardsSpeed = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _forwardSpeed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _forwardSpeed = -1;
        }
        else
        {
            _forwardSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _sidewaysSpeed = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _sidewaysSpeed = 1;
        }
        else
        {
            _sidewaysSpeed = 0;
        }
        Vector3 direction = new Vector3(_sidewaysSpeed, _upwardsSpeed, _forwardSpeed);
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
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

            _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
            _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            _camera.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
        }
    }
}
