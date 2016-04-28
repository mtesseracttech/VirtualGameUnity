using UnityEngine;
using System.Collections;

public class Tackle : MonoBehaviour
{
    public LayerMask CollisionMask;
    private float _speed = 10;
    private float _journeyLength;
    private bool _disantce;
    public bool TackleBool;
    Vector3 _startPos;

    void Start()
    {
        _disantce = false;
        TackleBool = false;

    }
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    void Update()
    {
        _startPos = transform.position;
        _journeyLength = _speed;
        CheckCollisions(_journeyLength);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _journeyLength,CollisionMask))
        {
            _disantce = false;
            TackleBool = false;
            if (Physics.Raycast(ray, out hit, 4, CollisionMask))
            {
                _disantce = true;
                if (Input.GetKey(KeyCode.E))
                {
                    _disantce = false;
                    TackleBool = true;
                    transform.position = Vector3.Lerp(_startPos, hit.collider.gameObject.transform.position, 0.03f);
                    if (Physics.Raycast(ray, out hit, 0.6f, CollisionMask))
                    {
                        Destroy(hit.transform.gameObject);
                        TackleBool = false;
                        _disantce = false;
                    } 
                }
            }
        }
    }
    void OnGUI()
    {
        if (_disantce)
        {
            GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 100, 120, 25), "Hold E");
        }
        
    }
}
