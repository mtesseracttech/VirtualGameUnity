using System;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public GameObject TargetObject;
    public int SightRange = 10;

    private Rigidbody _parent;
    private Rigidbody _target;
    private bool _targetInSight;
    private Vector3 _lastSeenPosition;

    // Use this for initialization
    private void Start()
    {
        _parent = GetComponent<Rigidbody>();
        _target = TargetObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (SeesTarget())
        {
            _lastSeenPosition = _target.position;
        }

    }

    private bool SeesTarget()
    {
        Vector3 differenceVec = _target.transform.position - _parent.transform.position;
        if (differenceVec.magnitude < SightRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(_parent.position, differenceVec, out hit, differenceVec.magnitude))
            {
                if (hit.collider.gameObject.tag != "Player")
                {
                    Debug.Log("Hit Something Else!");
                    return false;
                }
            }
            Debug.Log("Hit Player!");
            return true;
        }
        Debug.Log("Player is out of range!");
        return false;

    }
}