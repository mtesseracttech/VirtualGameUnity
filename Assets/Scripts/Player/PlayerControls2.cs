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
<<<<<<< HEAD
    public GameObject EnemyHitEffect;
    public GameObject DefaultHit;
    private LineRenderer lineRenderer;

    public Transform gunEnd;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;

    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for player movement //////////////////
    public Rigidbody rb;
    public int MaxMagnitude = 3;
    private float _maxMagnitude;
    public int accelerationValue;

    public float SlowDownFactor = 0.945f;
=======
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
>>>>>>> origin/master
    public int MovementSpeed = 1;
    public float JumpSpeed = 250;
    public float SlowDownFactor = 0.945f;
    public int MaxMovementSpeed = 3;
    private Rigidbody _rigidBody;
    private float _forwardSpeed = 0f;
    private float _sidewaysSpeed = 0f;

    private bool _grounded = true;

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

    public enum RotationAxes
    {
        MouseXAndY = 0, MouseX = 1, MouseY = 2
    }

    void Start()
	{
        //Getting Components
        _rigidBody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        _lineRenderer = GetComponent<LineRenderer>();

<<<<<<< HEAD
    ////////////// Variables used for player and camera rotation ///////
    public Transform camera;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -90F;
    public float maximumY = 90F;
    float rotationX = 0F;
    ///////////////////////////////////////////////////////////////////
	/// 
	void Start()
	{
		MaxMagnitude = 3;
        _maxMagnitude = MaxMagnitude;
	    accelerationValue = 25;
        SlowDownFactor = 0.945f;
		MovementSpeed = 2;
		JumpSpeed = 250;
		lineRenderer = GetComponent<LineRenderer>();
		source = GetComponent<AudioSource>();
		deltaTime = 0f;
		fireDelay = 1;
		countFireDelta = false;
		fireDelta = 0;
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		slowMotion = false;
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            slowMotion = !slowMotion;
        }
        if (slowMotion)
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
       // ammunation.text = " " + string.Format("{0:0.}", 1 / deltaTime)/*"Ammo: " + ammo*/;
=======
		_deltaTime = 0f;
        _fireDelta = 0f;
        _countFireDelta = false;
		_slowMotion = false;

        _rigidBody.freezeRotation = true;
    }
    void Update()
    {
        SlowdownCode();
        GUIInfo();
>>>>>>> origin/master
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
        if (Input.GetKeyDown(KeyCode.LeftShift)) _slowMotion = !_slowMotion;
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
                Gun.transform.LookAt(_hitInfo.point);
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
                    // Instantiating particles on enemy position
<<<<<<< HEAD
                   // Vector3 particlesPos = hitInfo.collider.gameObject.transform.position ;
                    //Instantiate(hitParticles, particlesPos, Quaternion.identity);
                    lineRenderer.SetPosition(0, gunEnd.position);
                    lineRenderer.SetPosition(1, hitInfo.point);
                    Instantiate(EnemyHitEffect, hitInfo.point, Quaternion.identity);
                }
                else
                {
                     lineRenderer.SetPosition(0, gunEnd.position);
                     lineRenderer.SetPosition(1, hitInfo.point);
                     Instantiate(DefaultHit, hitInfo.point, Quaternion.identity);
=======
                    Vector3 particlesPos = _hitInfo.collider.gameObject.transform.position ;
                    Instantiate(EnemyHitParticles, particlesPos, Quaternion.identity);
                }
                else
                {
                     _lineRenderer.SetPosition(0, GunEnd.position);
                     _lineRenderer.SetPosition(1, _hitInfo.point);
                     Instantiate(EnemyHitParticles, _hitInfo.point, Quaternion.identity);
>>>>>>> origin/master
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
<<<<<<< HEAD
        Vector3 _velocity = rb.velocity;
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            grounded = false;
            rb.AddRelativeForce(new Vector3(0, JumpSpeed, 0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            ForwardSpeed = MovementSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ForwardSpeed *= 2;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ForwardSpeed = -MovementSpeed;
        }
        else
        {
            ForwardSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            SidewaysSpeed = -MovementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
=======
        Vector3 velocity = _rigidBody.velocity;
        if (_grounded && Input.GetKeyDown(KeyCode.Space))
>>>>>>> origin/master
        {
            _grounded = false;
            _rigidBody.AddRelativeForce(new Vector3(0, JumpSpeed, 0));
        }

<<<<<<< HEAD
        Vector3 direction = new Vector3(SidewaysSpeed, 0, ForwardSpeed);
        if (direction == Vector3.zero)
        {
            if (grounded)
            {
                rb.velocity = rb.velocity * SlowDownFactor;
            }
            else
            {
                rb.velocity = rb.velocity * (SlowDownFactor + 0.03f);
            }
        }
        else
        {
            if (grounded)
            {
                _maxMagnitude = MaxMagnitude;
            }
            else
            {
                _maxMagnitude = MaxMagnitude / 2;
            }
            if (_velocity.magnitude <= _maxMagnitude)
            {
                rb.AddRelativeForce(direction * accelerationValue);
            }
            if (_velocity.magnitude > _maxMagnitude)
            {
                if (grounded)
                {
                    rb.velocity = rb.velocity * SlowDownFactor;
                }
                else
                {
                    rb.velocity = rb.velocity * (SlowDownFactor + 0.03f);
                }
=======
        if (Input.GetKey(KeyCode.W)) _forwardSpeed = MovementSpeed;
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
>>>>>>> origin/master
            }
        }
        //Debug.Log(rb.velocity.magnitude);
    }

    void PlayerAndCameraRotation()
    {
<<<<<<< HEAD
        // Rotation available on X-Axis and Y-Axis
        if (axes == RotationAxes.MouseXAndY)
        {
            //Value for angle on Y-Axis
            float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X")*sensitivityX;

            //Value for angle on X-Axis
            rotationX += Input.GetAxis("Mouse Y")*sensitivityY;
            rotationX = Mathf.Clamp(rotationX, minimumY, maximumY);

            //Player's rotation together with the camera (Y-Axis)
            transform.localEulerAngles = new Vector3(0, rotationY, 0);
            // Camera's rotation Up a Down (X-Axis)
            camera.localEulerAngles = new Vector3(-rotationX, 0, 0);
        }
        // Rotation available only on Y-Axis
        else if (axes == RotationAxes.MouseX)
        {
            // Rotates the player on Y-Axis
            transform.Rotate(0, Input.GetAxis("Mouse X")*sensitivityX, 0);
        }
    }

    public bool Grounded()
    {
        return grounded;
    }   
=======
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
>>>>>>> origin/master
}
