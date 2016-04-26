using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    public Rigidbody rb;
    public Transform camera;

    private string _state;
    private string _prevState;

    
    private Vector3 _prevPos;
    private Vector3 _position;
    private Vector3 _velocity;

    private bool _hasSword;
    private bool _hasSpear;
    private bool _hasBow;
    private string _equipped;
    private bool _readyToEquip;

    private bool _isInLake;
    public bool canSprint;
    private int _stamina;
    public bool hasKey;
    public bool canJump;
    private bool _jumped;

    public float ForwardSpeed;
    public float SidewaysSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    // Use this for initialization
    void Start() {



        rb = gameObject.GetComponent<Rigidbody>();

        _state = "Walk";
        _prevState = "Walk";

        _prevPos = _position;
        _position = gameObject.transform.position;
        _velocity = Vector3.zero;

        _hasSword = false;
        _hasSpear = false;
        _hasBow = false;
        _equipped = "Empty";
        _readyToEquip = true;

        _isInLake = false;
        canSprint = false;
        _stamina = 1000;
        hasKey = false;
        canJump = false;
        _jumped = false;
        
        ForwardSpeed = 0;
        SidewaysSpeed = 0;
        rotationSpeed = 2f;
        jumpSpeed = 150f;
    }

    // Update is called once per frame
    void Update()
    {
        SavePrevPos();

        switch (_state)
        {
            case "Walk":
                Move();
                break;
            case "Sprint":
                Sprint();
                break;

            default:
                break;
        }
        _position = gameObject.transform.position;
        Debug.Log("State: " + _state);
        Jump();
        Rotate();
    }

    void Move()
    {
        _velocity = rb.velocity;

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
        Vector3 direction = new Vector3(SidewaysSpeed, 0, ForwardSpeed);
        if (rb.velocity.magnitude <= 3)
        {
            rb.AddRelativeForce(direction * 10);
        }
        //Debug.Log("Walk: " + _velocity.magnitude);
    }
    void Sprint()
    {
        _velocity = rb.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            ForwardSpeed = 1.5f;
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
        Vector3 direction = new Vector3(SidewaysSpeed, 0, ForwardSpeed);
        if (rb.velocity.magnitude <= 5)
        {
            rb.AddRelativeForce(direction * 10);
        }
    }
    
    void Jump()
    {
        if (canJump == true && _jumped == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddRelativeForce(0, jumpSpeed, 0);
                _jumped = true;
            }
        }
    }
    void Rotate()
    {
        if (Input.GetAxis("Mouse Y") < 0)
        {
            camera.Rotate(rotationSpeed*0.75f, 0, 0);
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            camera.Rotate(-rotationSpeed*0.75f, 0, 0);
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            gameObject.transform.Rotate(0, rotationSpeed, 0);
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            gameObject.transform.Rotate(0, -rotationSpeed, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Rotate(0, -rotationSpeed, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.Rotate(0, rotationSpeed, 0);
        }
        //camera.rotation = Quaternion.Euler(new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0));
    }
    
    void SavePrevPos()
    {
        _prevPos = rb.transform.position;
        
    }
   
    public bool jumped
    {
        get { return _jumped; }
        set { _jumped = value; }
    }
    public string state
    {
        get { return _state; }
        set { _state = value; }
    }
    public string equipped
    {
        get { return _equipped; }
        set { _equipped = value; }
    }
    public int stamina
    {
        get { return _stamina; }
        set { _stamina = value; }
    }
    public bool isInLake
    {
        get { return _isInLake; }
        set { _isInLake = value; }
    }
    public bool hasSword
    {
        get { return _hasSword; }
        set { _hasSword = value; }
    }
    public bool hasSpear
    {
        get { return _hasSpear; }
        set { _hasSpear = value; }
    }
    public bool hasBow
    {
        get { return _hasBow; }
        set { _hasBow = value; }
    }
}
