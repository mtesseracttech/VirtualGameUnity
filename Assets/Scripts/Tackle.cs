using UnityEngine;
using System.Collections;

public class Tackle : MonoBehaviour
{
    public LayerMask collisionMask;
    private float speed = 10;
    private float journeyLength;
    private bool disantce;
    public bool tackleBool;
    Vector3 startPos;

    void Start()
    {
        disantce = false;
        tackleBool = false;

    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        startPos = transform.position;
        journeyLength = speed;
        CheckCollisions(journeyLength);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, journeyLength,collisionMask))
        {
            OnHitObject(hit);
            disantce = false;
            tackleBool = false;
            if (Physics.Raycast(ray, out hit, 4, collisionMask))
            {
                disantce = true;
                if (Input.GetKey(KeyCode.E))
                {
                    disantce = false;
                    tackleBool = true;
                    transform.position = Vector3.Lerp(startPos, hit.collider.gameObject.transform.position, 0.03f);
                    if (Physics.Raycast(ray, out hit, 0.6f, collisionMask))
                        Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    void OnHitObject(RaycastHit hit)
    {
       
    }

    void OnGUI()
    {
        if (disantce)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Hold E");
        }
        
    }
}
