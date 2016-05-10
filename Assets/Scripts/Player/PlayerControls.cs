using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {
   // public Text ammunation;
    private int ammo;
    private float deltaTime;
    public float range = 50;
    private bool slowMotion;

    ////////////// Variables used for Line of Aim Handling /////////////
    RaycastHit hitInfo;
    Ray ray;
    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for raycasting and shooting //////////
    public float fireDelay;
    private float fireDelta;
    private bool countFireDelta;

    public GameObject crosshair;
    public GameObject gun;
    public GameObject bulletPrefab;
    private GameObject bullet;
    private Vector3 prevRayCastPoint;

    public ParticleSystem SmokeParticleSystem;
    public GameObject hitParticles;
    public GameObject objectHitEffect;
    private LineRenderer lineRenderer;
    public Transform gunEnd;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    private AudioSource source;

    ////////////////////////////////////////////////////////////////////
    ////////////// Variables used for player movement //////////////////
    public Rigidbody rb;
    private int ForwardSpeed = 0;
    private int SidewaysSpeed = 0;
    private int UpwardsSpeed = 0;

    private bool grounded = true;
    ////////////////////////////////////////////////////////////////////

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
    float rotationY = 0F;
    public int MaxMovementSpeed = 10;
    public int MaxJumpSpeed = 100;
    ///////////////////////////////////////////////////////////////////
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
            grounded = true;
        }
    }
    void Start()
    {
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
    void PickUpHandler()
    {
        if (hitInfo.collider != null && hitInfo.collider.gameObject.tag == "magazine")
        {
            if ((hitInfo.collider.gameObject.transform.position - gameObject.transform.position).magnitude <= 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ammo += hitInfo.collider.gameObject.GetComponent<AmmoScript>().GetAmountOfBullets();
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }

    }

    void LineOfAimHandler()
    {
        // Makes the gun always point at the end of ray (crosshair point)
        ray = new Ray(camera.transform.position, crosshair.transform.position - camera.transform.position);
        if (Physics.Raycast(ray, out hitInfo,range))
        {           
            if (hitInfo.point != prevRayCastPoint)
            {
                gun.transform.LookAt(hitInfo.point);
                prevRayCastPoint = hitInfo.point;
            }
            Debug.DrawLine(camera.transform.position, hitInfo.point,Color.blue);
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
        lineRenderer.enabled = true;
        source.Play();
        yield return shotLength;
        lineRenderer.enabled = false;
    }
    void Shoot()
    {
        // Enable delaycounter (fireDelta++)
        if (Input.GetMouseButton(0) && !countFireDelta)
        {
            countFireDelta = true;
            PlayMuzzleFlash();
            
        }
        // Shoot a bullet if fireDelta = 0
        if (fireDelta == 0 && Input.GetMouseButton(0))
        {
            // Destroying enemies if rayHitInfo holds information about an enemy
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.tag == "enemy")
                {
                    Destroy(hitInfo.collider.gameObject);
                    // Instantiating particles on enemy position
                    Vector3 particlesPos = hitInfo.collider.gameObject.transform.position ;
                    Instantiate(hitParticles, particlesPos, Quaternion.identity);
                }
                else
                {
                     lineRenderer.SetPosition(0, gunEnd.position);
                     lineRenderer.SetPosition(1, hitInfo.point);
                     Instantiate(hitParticles, hitInfo.point, Quaternion.identity);
                }
                StartCoroutine(ShotEffect());
            }
            
            ///////////////////////////////////////////////////////////////////

            // Instantiating a bullet for visual effects
            Vector3 gunPos = gun.transform.position/*new Vector3(camera.position.x, camera.position.y-0.125f, camera.position.z)*/;
            Vector3 gunDirection = gun.transform.forward;
            Quaternion gunRotation = gun.transform.rotation;
            float spawnDistance = 0.055f;
            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;

            bullet = Instantiate(bulletPrefab, spawnPos, gunRotation) as GameObject;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, bullet.GetComponent<BulletScript>().force));
            bullet.GetComponent<BulletScript>().SetDistance((hitInfo.point - spawnPos).magnitude);
            ///////////////////////////////////////////////////////////////////
        }
        // Count delay for next shot
        if (countFireDelta)
        {
            fireDelta += Time.deltaTime;
            //Debug.Log(fireDelta + " Fire Delta");
        }
        // If (FireDelay) certain time has passed since last bullet was shot, fireDelta becomes 0 and next bullet can be shot
        if (fireDelta >= gun.GetComponent<GunScript>().FireDelay())
        {
            fireDelta = 0;
            countFireDelta = false;
        }
    }
    void PlayerMovement()
    {
        Vector3 _velocity = rb.velocity;
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            UpwardsSpeed = MaxJumpSpeed;
            grounded = false;
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
        if (rb.velocity.magnitude <= MaxMovementSpeed)
        {
            rb.AddRelativeForce(direction * MaxMovementSpeed*10);
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
    public bool Grounded()
    {
        return grounded;
    }

    
       
}
