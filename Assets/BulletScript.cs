using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    private Rigidbody rb;
    public int force = 50;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            rb.velocity = Vector3.zero;
        }
    }
}
