using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMotor))]
//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControler : MonoBehaviour
{
    //handels all the inputs 
    // Use this for initialization
    //protected but still will show up in unity. same as private
    [Header("MovementBehaviour settings:")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sensitivity = 3f; //for camera movement
    [SerializeField] private float _thrusterForce = 10f;
   // [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f;
    //  [SerializeField] private Animator animator;
    private RaycastHit _hit;

    private PlayerMotor _motor;
    private ConfigurableJoint joint; //for jumping and smoothly going down
    [SerializeField] private LayerMask _environmentMask; //cheking for layers
    public AudioClip[] Footsteps;                       //sound for steps
    private AudioSource _aud;   
    private float _footstepSpeed = 1f;
    private bool _isgrounded  = true;

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 2f;
    [SerializeField]
    private float jointMaxForce = 1f;

    private void Start()
    {
        _motor = GetComponent<PlayerMotor>();
        //animator = GetComponent<Animator>();
        joint = GetComponent<ConfigurableJoint>();
        _aud = GetComponent<AudioSource>();
        SetJointSettings(jointSpring);
    }
    //not working
    void OnCotrollerColliderHit(ControllerColliderHit hit)
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5
            && hit.gameObject.tag == "Ground" && _isgrounded||
            controller.isGrounded && controller.velocity.magnitude < 7 && controller.velocity.magnitude > 5
            && hit.gameObject.tag == "Untagged" && _isgrounded)
            WalkOnGround();
    }

    IEnumerator WalkOnGround()
    {
        _isgrounded = false;
        _aud.clip = Footsteps[Random.Range(0, Footsteps.Length)];
        _aud.volume = 1;
        _aud.Play();
        yield return new WaitForSeconds(_footstepSpeed);
        _isgrounded = true;
    }
    //-------------------------------------------------------------------------------------//
    //       Ray casting
    //-------------------------------------------------------------------------------------//
    void CheckRayCast()
    {
        
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 10f, _environmentMask)) //casting ray
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
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
        _footstepSpeed = 0.5f;
        //animation movement
       // animator.SetFloat("ForwardVelocity", zMove);

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

       // ApplyThursters();
        if (Input.GetKey("left shift"))
        {
            Vector3 movefastforward = transform.forward;
            Vector3 velocity1  = movefastforward * _runSpeed;
            _motor.Move(velocity1);
            _footstepSpeed = 0.25f;
        }
    }

    void ApplyThursters()
    {
      
        //calculate thruster froce on playrs input
        Vector3 thrusterForce = Vector3.zero;
        //apply thurster force
        if (Input.GetButton("Jump"))
        {
            thrusterForce = Vector3.up * _thrusterForce;
            SetJointSettings(0f);
        }
        else
            SetJointSettings(jointSpring);
        _motor.ApplyThruster(thrusterForce);
        
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

    private void Update()
    {
        CheckRayCast();
        MoveBehaviour();  
    }

   


}
