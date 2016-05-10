using UnityEngine;
using System.Collections;

public class enemyScript : MonoBehaviour {
    //////// Levitate variables ////////
    private bool up;
    private int upLimit;
    private int upCounter;
    ////////////////////////////////////
    //////// Laser variables ///////////
    public LineRenderer laser;
    public GameObject player;
    ////////////////////////////////////
    // Use this for initialization
    void Start () {
        up = false;
        upLimit = 50;
        upCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Levitate();
        laser.SetPosition(1, transform.InverseTransformVector(player.transform.position - transform.position));
        transform.LookAt(player.transform);
    }
    void Levitate()
    {
        Vector3 stepVector = new Vector3(0, 0.002f, 0);
        if (up)
        {
            gameObject.transform.position += stepVector;
            upCounter += 1;
            if (upCounter >= upLimit)
            {
                up = !up;
                upCounter = 0;
            }
        }
        if (!up)
        {
            gameObject.transform.position -= stepVector;
            upCounter += 1;
            if (upCounter >= upLimit)
            {
                up = !up;
                upCounter = 0;
            }
        }
    }
   
}
