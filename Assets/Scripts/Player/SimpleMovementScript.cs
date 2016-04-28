using UnityEngine;

public class SimpleMovementScript : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    ////////////////////////////////////////////////////////////////////

    ////////////// Variables used for player and camera rotation ///////
    [SerializeField] private Transform _camera;

    private int _forwardSpeed;
    private float _rotationY;
    private int _sidewaysSpeed;
    private int _upwardsSpeed;
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float MaximumX = 360F;
    public float MaximumY = 60F;
    public float MinimumX = -360F;
    public float MinimumY = -60F;
    ////////////// Variables used for player movement //////////////////
    public Rigidbody rb;
    public float SensitivityX = 15F;
    public float SensitivityY = 15F;


    // Use this for initialization
    private void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMovement();
        PlayerAndCameraRotation();
    }

    private void PlayerMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _upwardsSpeed = 10;
        else _upwardsSpeed = 0;

        if (Input.GetKey(KeyCode.W)) _forwardSpeed = 1;
        else if (Input.GetKey(KeyCode.S)) _forwardSpeed = -1;
        else _forwardSpeed = 0;

        if (Input.GetKey(KeyCode.A)) _sidewaysSpeed = -1;
        else if (Input.GetKey(KeyCode.D)) _sidewaysSpeed = 1;
        else _sidewaysSpeed = 0;

        var direction = new Vector3(_sidewaysSpeed, _upwardsSpeed, _forwardSpeed);
        if (rb.velocity.magnitude <= 3) rb.AddRelativeForce(direction * 10);
    }

    private void PlayerAndCameraRotation()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X")*SensitivityX;

            _rotationY += Input.GetAxis("Mouse Y")*SensitivityY;
            _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            _camera.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X")*SensitivityX, 0);
        }
    }
}