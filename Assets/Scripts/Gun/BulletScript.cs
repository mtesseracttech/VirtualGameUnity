using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public GameObject bulletHolePrefab;
    private Rigidbody rb;
    public int force = 50;

    private Vector3 startPos;
    private float distance;

    private bool createHole;
    private Vector3 impactPos;
    private Quaternion impactRot;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If the bullet traveles more than distance to the object the gun was pointing at, the bullet will destroy itself
        if ((gameObject.transform.position - startPos).magnitude >= distance)
        {
            if (createHole)
            {
                Instantiate(bulletHolePrefab, impactPos, impactRot);
            }
            //Debug.Log((gameObject.transform.position-startPos).magnitude + " | " + distance + " | " + (impactPos - startPos).magnitude);
            Destroy(gameObject);

        }
        else if (rb.velocity.magnitude <= 0.1f)
        {
            if (createHole)
            {
                Instantiate(bulletHolePrefab, impactPos, impactRot);
            }
            Destroy(gameObject);
        }
    }
    public void CreateImpactPosImpactRot(bool pCreateHole, Vector3 pImpactPos, Quaternion pImpactRot)
    {
        createHole = pCreateHole;
        impactPos = pImpactPos;
        impactRot = pImpactRot;
    }
    public void SetDistance(float pDistance)
    {
        distance = pDistance;
    }
}
