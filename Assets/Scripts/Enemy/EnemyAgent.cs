using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    private readonly Dictionary<Type, AbstractEnemyState> _stateCache = new Dictionary<Type, AbstractEnemyState>();
    private Vector3 _debugLine;
    private AbstractEnemyState _state;
    public int SightConeAngle = 90;
    public int SightRange = 10;
    public GameObject TargetObject;
    public float SlerpSpeed = 0.1f;

    public Rigidbody Parent { get; private set; }
    public Rigidbody Target { get; private set; }
    public bool EnteredNewState { get; set; }
    public Vector3 LastSeenTargetPosition { get; private set; }
    public bool SeesTarget { get; private set; }


    // Use this for initialization
    private void Start()
    {
        Parent = GetComponent<Rigidbody>();
        Target = TargetObject.GetComponent<Rigidbody>();

        _stateCache[typeof (PatrolState)] = new PatrolState(this, Parent.rotation);
        _stateCache[typeof (ChaseState)] = new ChaseState(this);
        _stateCache[typeof (AttackState)] = new AttackState(this);
        _stateCache[typeof (ReturnState)] = new ReturnState(this, Parent.position, Parent.rotation);

        SetState(typeof (PatrolState));
    }

    public void SetState(Type pState)
    {
        Debug.Log("Switching state to:" + pState.FullName);
        EnteredNewState = true;
        _state = _stateCache[pState];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (LookForTarget())
        {
            LastSeenTargetPosition = Target.position;
            var differenceVector = LastSeenTargetPosition - Parent.position;
            Parent.transform.rotation = Quaternion.Slerp(Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), SlerpSpeed);
            Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, Parent.transform.rotation.eulerAngles.y, 0f));
            Debug.DrawLine(Parent.position, Target.position, Color.red);
            SeesTarget = true;
        }
        else
        {
            SeesTarget = false;
        }
        _state.Update();
        DebugCode();
    }

    private void DebugCode()
    {
        Debug.DrawLine(Parent.position, Parent.position + Parent.transform.forward, Color.blue);

        if (Input.GetKeyDown("g"))
        {
            Debug.Log("Currently in state: " + _state);
        }
    }


    // Update is called once per frame
    private bool LookForTarget()
    {
        var differenceVec = Target.transform.position - Parent.transform.position;
        if (differenceVec.magnitude < SightRange) //Sees if Target is even in range
        {
            var targetAngle = Vector3.Angle(differenceVec, Parent.transform.forward);
            if (targetAngle > SightConeAngle/2) //Sees if Target is in sight cone
            {
                Debug.Log("Player out of sight cone!");
                return false;
            }
            RaycastHit hit;
            if (Physics.Raycast(Parent.position, differenceVec, out hit, differenceVec.magnitude))
            {
                if (hit.collider.gameObject.tag != "Player") //Checks if anything is between the Target and the Parent
                {
                    Debug.Log("Something is between me and the Player");
                    return false;
                }
            }
            Debug.Log("I can see the Player!");
            return true;
        }
        Debug.Log("Player is out of range!");
        return false;
    }
}