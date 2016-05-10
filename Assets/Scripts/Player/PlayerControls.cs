using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
   // public Text ammunation;
    private int ammo;
    private float deltaTime;

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

    public GameObject particlePrefab;
    public ParticleSystem muzzleFlash;
    public GameObject objectHitEffect;
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
    ///////////////////////////////////////////////////////////////////
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) slowMotion = !slowMotion;
        if (slowMotion) Time.timeScale = 0.5f;
        else Time.timeScale = 1f;

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
        if (collision.gameObject.CompareTag("floor")) grounded = true;
    }

    void Start()
    {
        deltaTime = 0f;
        fireDelay = 1;
        countFireDelta = false;
        fireDelta = 0;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
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
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.point != prevRayCastPoint)
            {  
                gun.transform.LookAt(hitInfo.point);
                prevRayCastPoint = hitInfo.point;
            }
            Debug.DrawLine(camera.transform.position, hitInfo.point, Color.black);
        }
    }

    void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
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
                    // Instantiating particles on target position
                    //Vector3 particlesPos = hitInfo.collider.gameObject.transform.position + new Vector3(0, 0.2f, 0);
                    Instantiate(particlePrefab, hitInfo.point, Quaternion.identity);
                }
                else
                {
                    Vector3 muzzleFlashpos = hitInfo.collider.gameObject.transform.position//hitInfo.point;
                    Instantiate(objectHitEffect, muzzleFlashpos, Quaternion.identity);
                }
            }

            // Instantiating a bullet for visual effects
            Vector3 gunPos = gun.transform.position/*new Vector3(camera.position.x, camera.position.y-0.125f, camera.position.z)*/;
            Vector3 gunDirection = gun.transform.forward;
            Quaternion gunRotation = gun.transform.rotation;
            float spawnDistance = 0.055f;
            Vector3 spawnPos = gunPos + gunDirection * spawnDistance;
            Debug.Log("Bullet spawn position: " + spawnPos);

            bullet = Instantiate(bulletPrefab, spawnPos, gunRotation) as GameObject;

            bullet.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, bullet.GetComponent<BulletScript>().force));
            bullet.GetComponent<BulletScript>().SetDistance((hitInfo.point - spawnPos).magnitude);
        }

        
        if (countFireDelta) // Count delay for next shot
        {
            fireDelta += Time.deltaTime;
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
            UpwardsSpeed = 10;
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
        if (rb.velocity.magnitude <= 1)
        {
            rb.AddRelativeForce(direction * 10);
        }
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
