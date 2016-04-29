using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    private Rigidbody rb;
    public int force = 50;

    private Vector3 startPos;
    private float distance;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        startPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        // If the bullet traveles more than distance to the object the gun was pointing at, the bullet will destroy itself
        if ((gameObject.transform.position - startPos).magnitude > distance)
        {
            Destroy(gameObject);
        }
        else if (rb.velocity.magnitude <= 0.1f)
        {
            Destroy(gameObject);
        }
	}
    public void SetDistance(float pDistance)
    {
        distance = pDistance;
    }
}
