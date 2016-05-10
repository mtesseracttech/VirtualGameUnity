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
    public NavigationPath PatrolPath = null;
    //public GameObject Gun;
    //public GameObject BulletPrefab;
    //public GameObject PlayerImpactParticles;

    public NavMeshAgent NavAgent { get; set; }
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
        NavAgent = GetComponent<NavMeshAgent>();

        _stateCache[typeof (PatrolState)] = new PatrolState(this, Parent.rotation, PatrolPath);
        _stateCache[typeof (ChaseState)] = new ChaseState(this);
        _stateCache[typeof (AttackState)] = new AttackState(this);
        _stateCache[typeof (ReturnState)] = new ReturnState(this, Parent.position, Parent.rotation);
        _stateCache[typeof (LookoutState)] = new LookoutState(this);

        SetState(typeof (PatrolState));
    }

    public void SetState(Type pState)
    {
        Debug.Log("Switching state to:" + pState.FullName);
        EnteredNewState = true;
        NavAgent.Stop();
        SetSeeTarget();
        _state = _stateCache[pState];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        DebugCode();
        _state.Update();
        SetSeeTarget();
    }

    private void SetSeeTarget()
    {
        if (LookForTarget())
        {
            LastSeenTargetPosition = Target.position;
            Debug.DrawLine(Parent.position, Target.position, Color.yellow);
            SeesTarget = true;
        }
        else
        {
            SeesTarget = false;
        }
    }

    private void DebugCode()
    {
        Debug.DrawLine(Parent.transform.position, Parent.transform.position + Parent.transform.forward, Color.blue);
        if (Input.GetKeyDown("g"))
        {
            Debug.Log("Currently in state: " + _state);
        }

    }


    // Update is called once per frame
    private bool LookForTarget()
    {
        var differenceVec = Target.transform.position - Parent.transform.position;
        //Debug.Log(differenceVec.magnitude);
        if (differenceVec.magnitude < SightRange) //Sees if Target is even in range
        {
            var targetAngle = Vector3.Angle(differenceVec, Parent.transform.forward);
            //Debug.Log("Angle between Parent and Target: " + targetAngle);
            if (targetAngle > SightConeAngle/2)
            {
                return false; //Sees if Target is in sight cone
            }
            RaycastHit hit;
            if (Physics.Raycast(Parent.position, differenceVec, out hit, differenceVec.magnitude))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag != "Player") return false; //Checks if anything is between the Target and the Parent
            }
            return true;
        }
        return false;
    }

    public void CreateParticles(GameObject particles, Vector3 position) {Instantiate(particles, position, Quaternion.identity);}
}