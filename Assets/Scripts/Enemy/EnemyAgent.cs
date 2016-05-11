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
    public GameObject MuzzleFlash;
    public GameObject BloodParticles;
    public GameObject ImpactParticles;

    Vector3 _stepVector = new Vector3(0, 0.002f, 0);
    private bool _movingUp = false;
    private int _upLimit = 50;
    private int _upCounter = 0;



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

        List<GameObject> childObjects = GetChildrenComponents();

        _stateCache[typeof (PatrolState)] = new PatrolState(this, Parent.rotation, PatrolPath);
        _stateCache[typeof (ChaseState)] = new ChaseState(this);
        _stateCache[typeof (AttackState)] = new AttackState(this, MuzzleFlash, BloodParticles, ImpactParticles, childObjects.Find(child => child.name == "DroneTurretPoint"));
        _stateCache[typeof (ReturnState)] = new ReturnState(this, Parent.position, Parent.rotation);
        _stateCache[typeof (LookoutState)] = new LookoutState(this);

        SetState(typeof (PatrolState));
    }

    private List<GameObject> GetChildrenComponents()
    {
        List<GameObject> childrenObjects = new List<GameObject>();
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            if (Parent.transform.GetChild(i).name == "DroneTurretPoint") childrenObjects.Add(Parent.transform.GetChild(i).gameObject);
        }
        return childrenObjects;
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
        Levitate();
        SetSeeTarget();
        _state.Update();
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

    public void CreateParticles(GameObject particles, Vector3 position)
    {
        Instantiate(particles, position, Quaternion.identity);
    }

    public void CreateParticlesRotated(GameObject particles, Vector3 position, Quaternion rotation)
    {
        Instantiate(particles, position, rotation);
    }

    void Levitate()
    {
        if (_movingUp)
        {
            Parent.transform.position += _stepVector;
            Debug.Log("Moving Up");
        }
        else
        {
            Debug.Log("Moving Down");
            Parent.transform.position -= _stepVector;
        }

        _upCounter += 1;
        if (_upCounter >= _upLimit)
        {
            _movingUp = !_movingUp;
            _upCounter = 0;
        }
    }
}