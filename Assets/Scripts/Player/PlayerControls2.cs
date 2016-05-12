using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControls2 : MonoBehaviour {
   // public Text ammunation;
    private int _ammo;
    private float _deltaTime;
    public float Range = 50;
    private bool _slowMotion;

    // Variables used for Line of Aim Handling
    RaycastHit _hitInfo;
    Ray _ray;

    // Variables used for raycasting and shooting
    public GameObject Gun;
    public Transform GunEnd;
    public GameObject BulletPrefab;
    public float FireDelay;

    public GameObject Crosshair;

    public ParticleSystem SmokeParticleSystem;

    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;

    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for player movement //////////////////
    public Rigidbody rb;
    public int MaxMagnitude = 3;
    private float _maxMagnitude;
    public int accelerationValue;


    public GameObject EnemyHitParticles;
    public GameObject DefaultHitParticles;

    private GameObject _bullet;
    private Vector3 _prevRayCastPoint;
    private LineRenderer _lineRenderer;
    private WaitForSeconds _shotLength = new WaitForSeconds(.07f);
    private AudioSource _source;
    private float _fireDelta;
    private bool _countFireDelta;

    // Variables used for player movement

    public int MovementSpeed = 1;
    public float JumpSpeed = 250;
    public float SlowDownFactor = 0.945f;
    public int MaxMovementSpeed = 3;
    private Rigidbody _rigidBody;
    private float _forwardSpeed = 0f;
    private float _sidewaysSpeed = 0f;

    private bool _grounded = true;

    public enum RotationAxes
    {
        MouseXAndY = 0, MouseX = 1, MouseY = 2
    }

    // Variables used for player and camera rotation
    public Transform Camera;
    public RotationAxes Axes = RotationAxes.MouseXAndY;
    public float SensitivityX = 15F;
    public float SensitivityY = 15F;
    public float MinimumX = -360F;
    public float MaximumX = 360F;
    public float MinimumY = -90F;
    public float MaximumY = 90F;
    private float _rotationY = 0f;

   

    void Start()
    {
        //Getting Components
        _rigidBody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        SlowdownCode();
        GUIInfo();
        LineOfAimHandler();
        PlayerMovement();
        PlayerAndCameraRotation();
        Shoot();
        PickUpHandler();
    }

    private void GUIInfo()
    {
        //ammunation.text = " " + string.Format("{0:0.}", 1 / deltaTime)/*"Ammo: " + ammo*/;
    }


    private void SlowdownCode()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand)) _slowMotion = !_slowMotion;
        if (_slowMotion) Time.timeScale = 0.5f;
        else Time.timeScale = 1f;
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            _grounded = true;
        }
    }

    void PickUpHandler()
    {
        if (_hitInfo.collider != null && _hitInfo.collider.gameObject.tag == "magazine")
        {
            if ((_hitInfo.collider.gameObject.transform.position - gameObject.transform.position).magnitude <= 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _ammo += _hitInfo.collider.gameObject.GetComponent<AmmoScript>().GetAmountOfBullets();
                    Destroy(_hitInfo.collider.gameObject);
                }
            }
        }
    }

    void LineOfAimHandler() // Makes the gun always point at the end of ray (crosshair point)
    {
        _ray = new Ray(Camera.transform.position, Crosshair.transform.position - Camera.transform.position);
        if (Physics.Raycast(_ray, out _hitInfo,Range))
        {           
            if (_hitInfo.point != _prevRayCastPoint)
            {
                if ((Camera.transform.position - _hitInfo.point).magnitude >= 2f)
                {
                    Gun.transform.LookAt(_hitInfo.point);
                }
                _prevRayCastPoint = _hitInfo.point;
            }
            Debug.DrawLine(Camera.transform.position, _hitInfo.point,Color.blue);
        }
    }

    void PlayMuzzleFlash()
    {
        if (SmokeParticleSystem != null) SmokeParticleSystem.Play();
    }

    private IEnumerator ShotEffect()
    {
        _lineRenderer.enabled = true;
        _source.Play();
        yield return _shotLength;
        _lineRenderer.enabled = false;
    }

    void Shoot()
    {
        // Enable delaycounter (fireDelta++)
        if (Input.GetMouseButton(0) && !_countFireDelta)
        {
            _countFireDelta = true;
            PlayMuzzleFlash();
            
        }
        // Shoot a bullet if fireDelta = 0
        if (_fireDelta == 0.0f && Input.GetMouseButton(0))
        {
            // Destroying enemies if rayHitInfo holds information about an enemy
            if (Physics.Raycast(_ray, out _hitInfo))
            {
                if (_hitInfo.collider.gameObject.tag == "enemy")
                {
                    Destroy(_hitInfo.collider.gameObject);
                    _lineRenderer.SetPosition(0, GunEnd.position);
                    _lineRenderer.SetPosition(1, _hitInfo.point);
                    Instantiate(DefaultHitParticles, _hitInfo.point, Quaternion.identity);
                }
               
                else
                {
                     _lineRenderer.SetPosition(0, GunEnd.position);
                     _lineRenderer.SetPosition(1, _hitInfo.point);
                     Instantiate(EnemyHitParticles, _hitInfo.point, Quaternion.identity);
                }
                StartCoroutine(ShotEffect());
            }

            // Instantiating a bullet for visual effects
            Vector3 gunPos = Gun.transform.position/*new Vector3(camera.position.x, camera.position.y-0.125f, camera.position.z)*/;
            Vector3 gunDirection = Gun.transform.forward;
            Quaternion gunRotation = Gun.transform.rotation;
            float spawnDistance = 0.055f;
            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;

            _bullet = Instantiate(BulletPrefab, spawnPos, gunRotation) as GameObject;
            if (_bullet != null)
            {
                _bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, _bullet.GetComponent<BulletScript>().force));
                _bullet.GetComponent<BulletScript>().SetDistance((_hitInfo.point - spawnPos).magnitude);
            }
        }
        
        // Count delay for next shot
        if (_countFireDelta) _fireDelta += Time.deltaTime;

        // If (FireDelay) certain time has passed since last bullet was shot, fireDelta becomes 0 and next bullet can be shot
        if (_fireDelta >= Gun.GetComponent<GunScript>().FireDelay())
        {
            _fireDelta = 0.0f;
            _countFireDelta = false;
        }
    }

    private void PlayerMovement()
    {

        Vector3 velocity = _rigidBody.velocity;
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            _grounded = false;
            _rigidBody.AddRelativeForce(new Vector3(0, JumpSpeed, 0));
        }

        if (Input.GetKey(KeyCode.W))
        {
            _forwardSpeed = MovementSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _forwardSpeed *= 2;
            }
        }
        else if (Input.GetKey(KeyCode.S)) _forwardSpeed = -MovementSpeed;
        else _forwardSpeed = 0;

        if (Input.GetKey(KeyCode.A)) _sidewaysSpeed = -MovementSpeed;
        else if (Input.GetKey(KeyCode.D)) _sidewaysSpeed = MovementSpeed;
        else _sidewaysSpeed = 0;

        Vector3 direction = new Vector3(_sidewaysSpeed, 0, _forwardSpeed);

        if (direction == Vector3.zero) _rigidBody.velocity = _rigidBody.velocity * SlowDownFactor;
        else
        {
            if (velocity.magnitude <= MaxMovementSpeed)
            {
                _rigidBody.AddRelativeForce(direction * 50);
            }
            if (velocity.magnitude > MaxMovementSpeed)
            {
                _rigidBody.velocity = _rigidBody.velocity * SlowDownFactor;
            }
        }
    }

    void PlayerAndCameraRotation()
    {

        if (Axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * SensitivityX;

            _rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
            _rotationY = Mathf.Clamp(_rotationY, MinimumY, MaximumY);

            transform.localEulerAngles = new Vector3(0, rotationX, 0);
            Camera.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }
        else if (Axes == RotationAxes.MouseX) transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
    }

    public bool Grounded
    {
        get
        {
            return _grounded;
        }
       
    }
}
