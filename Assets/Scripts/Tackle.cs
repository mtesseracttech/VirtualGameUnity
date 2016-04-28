using UnityEngine;
using System.Collections;

public class Tackle : MonoBehaviour
{
    public LayerMask collisionMask;
    private float speed = 10;
    private float journeyLength;
    private bool disantce = false;
    float lerpTime = 1f;
    float currentLerpTime;
    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        disantce = false;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        
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
            if (Physics.Raycast(ray, out hit, 4, collisionMask))
            {
                disantce = true;
                if (Input.GetKeyUp(KeyCode.E))
                {
                    disantce = false;
                    float perc = currentLerpTime / lerpTime;
                    transform.position = Vector3.Lerp(startPos,hit.collider.gameObject.transform.position,perc);
                }
            }
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        print(" we hit " + hit.collider.gameObject.name + " distance " + hit.distance);
        //GameObject.Destroy(gameObject);
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "E"))
       // {
            //GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Press E");
        //    disantce = !disantce;
      //  }
        if (disantce)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Press E");
        }
        
    }
}
