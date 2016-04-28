using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControler : MonoBehaviour
{
    //handels all the inputs 
    // Use this for initialization
    //protected but still will show up in unity. same as private
    [Header("MovementBehaviour settings:")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _sensitivity = 1f; //for camera movement
    [SerializeField] private float _runSpeed = 2f;
    private RaycastHit _hit;

    private PlayerMotor _motor;
    public GameObject bulletPrefab;
    private GameObject bullet;
    public GameObject gun;
    public Rigidbody rb;
    

    private void Start()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 gunPos = gun.transform.position;
            Vector3 gunDirection = gun.transform.forward;
            Quaternion gunRotation = gun.transform.rotation;
            float spawnDistance = 0.055f;

            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;

            bullet = Instantiate(bulletPrefab, spawnPos, gunRotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 1));

        }
    }

    void MoveBehaviour()
    {
        //calculate velocity as vector
        //GetAxisRaw has no smoothing. Will do that ourself for slower movement .
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        //vector with values  (1,0,0)
        Vector3 moveHorizontal = transform.right * xMove;

        //vector with values  (0,0,1)forward
        //vector with values  (0,0,0)not moving
        //vector with values  (0,0,-1)backwards
        Vector3 moveVertical = transform.forward * zMove;

        //lenght doesn't matter. totall lenght should be one. we won't get a different speed. 
        Vector3 velocity = (moveHorizontal + moveVertical) * _speed;

        //apply movement
        _motor.Move(velocity);

        //calculate rotation as vector : turning around
        float yRotation = Input.GetAxisRaw("Mouse X");
        //want only camera to be affected
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * _sensitivity;
        //apply rotaion
        _motor.Rotate(rotation);

        //calculate camera as vector : turning around
        float xRotation = Input.GetAxisRaw("Mouse Y");
        //want only camera to be affected
        float cameraRotaionX = xRotation * _sensitivity;
        //apply rotaion
        _motor.RotateCamera(cameraRotaionX);

        if (Input.GetKey("left shift"))
        {
            Vector3 movefastforward = transform.forward;
            Vector3 velocity1  = movefastforward * _runSpeed;
            _motor.Move(velocity1);
        }
    }



    private void Update()
    {
        MoveBehaviour();  
    }

}
