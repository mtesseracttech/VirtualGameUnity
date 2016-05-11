using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {
   // public Text ammunation;
    private int _ammo;
    private float _deltaTime;
    public float Range = 50;
    private bool _slowMotion;

    ////////////// Variables used for Line of Aim Handling /////////////
    RaycastHit _hitInfo;
    Ray _ray;
    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for raycasting and shooting //////////
    public float FireDelay;
    private float _fireDelta;
    private bool _countFireDelta;

    public GameObject Crosshair;
    public GameObject Gun;
    public GameObject BulletPrefab;
    private GameObject bullet;
    private Vector3 _prevRayCastPoint;

    public ParticleSystem SmokeParticleSystem;
    public GameObject HitParticles;
    public GameObject ObjectHitEffect;
    private LineRenderer _lineRenderer;
    public Transform GunEnd;
    private readonly WaitForSeconds _shotLength = new WaitForSeconds(.07f);
    private AudioSource _source;

    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for player movement //////////////////
    public Rigidbody Rb;
    private int _forwardSpeed = 0;
    private int _sidewaysSpeed = 0;
    private int _upwardsSpeed = 0;

    private bool _grounded = true;
    public int MaxSpeed = 5;
    public int MaxJumpSpeed = 10;

    ////////////////////////////////////////////////////////////////////

    ////////////// Variables used for player and camera rotation ///////
    public Transform camera;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float SensitivityX = 15F;
    public float SensitivityY = 15F;
    public float MinimumX = -360F;
    public float MaximumX = 360F;
    public float MinimumY = -90F;
    public float MaximumY = 90F;
    float _rotationY = 0F;
    ///////////////////////////////////////////////////////////////////
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _slowMotion = !_slowMotion;
        }
        if (_slowMotion)
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
       // ammunation.text = " " + string.Format("{0:0.}", 1 / deltaTime)/*"Ammo: " + ammo*/;
        LineOfAimHandler();
        PlayerMovement();
        PlayerAndCameraRotation();
        Shoot();
        PickUpHandler();
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            _grounded = true;
        }
    }
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _source = GetComponent<AudioSource>();
        _deltaTime = 0f;
        FireDelay = 1;
        _countFireDelta = false;
        _fireDelta = 0;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
        _slowMotion = false;
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

    void LineOfAimHandler()
    {
        // Makes the gun always point at the end of ray (crosshair point)
        _ray = new Ray(camera.transform.position, Crosshair.transform.position - camera.transform.position);
        if (Physics.Raycast(_ray, out _hitInfo,Range))
        {           
            if (_hitInfo.point != _prevRayCastPoint)
            {
                Gun.transform.LookAt(_hitInfo.point);
                _prevRayCastPoint = _hitInfo.point;
            }
            Debug.DrawLine(camera.transform.position, _hitInfo.point,Color.blue);
        }
        ///////////////////////////////////////////////////////////////////
    }

    void PlayMuzzleFlash()
    {
        if (SmokeParticleSystem != null)
        {
            SmokeParticleSystem.Play();
        }
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
        if (_fireDelta == 0 && Input.GetMouseButton(0))
        {
            // Destroying enemies if rayHitInfo holds information about an enemy
            if (Physics.Raycast(_ray, out _hitInfo))
            {
                if (_hitInfo.collider.gameObject.tag == "enemy")
                {
                    Destroy(_hitInfo.collider.gameObject);
                    // Instantiating particles on enemy position
                    Vector3 particlesPos = _hitInfo.collider.gameObject.transform.position ;
                    Instantiate(HitParticles, particlesPos, Quaternion.identity);
                }
                else
                {
                     _lineRenderer.SetPosition(0, GunEnd.position);
                     _lineRenderer.SetPosition(1, _hitInfo.point);
                     Instantiate(HitParticles, _hitInfo.point, Quaternion.identity);
                }
                StartCoroutine(ShotEffect());
            }
            
            ///////////////////////////////////////////////////////////////////

            // Instantiating a bullet for visual effects
            Vector3 gunPos = Gun.transform.position/*new Vector3(camera.position.x, camera.position.y-0.125f, camera.position.z)*/;
            Vector3 gunDirection = Gun.transform.forward;
            Quaternion gunRotation = Gun.transform.rotation;
            float spawnDistance = 0.055f;
            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;

            bullet = Instantiate(BulletPrefab, spawnPos, gunRotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, bullet.GetComponent<BulletScript>().force));
            bullet.GetComponent<BulletScript>().SetDistance((_hitInfo.point - spawnPos).magnitude);
            ///////////////////////////////////////////////////////////////////
        }
        // Count delay for next shot
        if (_countFireDelta)
        {
            _fireDelta += Time.deltaTime;
            //Debug.Log(fireDelta + " Fire Delta");
        }
        // If (FireDelay) certain time has passed since last bullet was shot, fireDelta becomes 0 and next bullet can be shot
        if (_fireDelta >= Gun.GetComponent<GunScript>().FireDelay())
        {
            _fireDelta = 0;
            _countFireDelta = false;
        }
    }
    void PlayerMovement()
    {
        Vector3 _velocity = Rb.velocity;
        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _upwardsSpeed = MaxJumpSpeed;
            _grounded = false;
        }
        else
        {
            _upwardsSpeed = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _forwardSpeed = 20;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _forwardSpeed = -5;
        }
        else
        {
            _forwardSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _sidewaysSpeed = -2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _sidewaysSpeed = 5;
        }
        else
        {
            _sidewaysSpeed = 0;
        }
        Vector3 direction = new Vector3(_sidewaysSpeed, _upwardsSpeed, _forwardSpeed);
        if (Rb.velocity.magnitude <= MaxSpeed)
        {
            Rb.AddRelativeForce(direction * MaxSpeed * 10);
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
            camera.localEulerAngles = new Vector3(-_rotationY, 0, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * SensitivityX, 0);
        }
    }
    public bool Grounded()
    {
        return _grounded;
    }    
}
