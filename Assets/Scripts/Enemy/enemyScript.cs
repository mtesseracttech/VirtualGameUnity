using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public LineRenderer LaserLine;
    private Vector3 _turretPoint;
    private Transform _turret;
    private EnemyAgent _agent;
    private Renderer _laserRenderer;


    void Awake()
    {
        _laserRenderer = LaserLine.transform.GetComponent<Renderer>();
        _agent = GetComponent<EnemyAgent>();
        GetChildrenComponents();
    }

    private void Update()
    {
        Laser();
    }

    private void Laser()
    {
        _turretPoint = _turret.transform.position;
        if (_agent.GetState() is AttackState)
        {
            Debug.Log("SHOULD BE DRAWING A LASER");
            LaserLine.SetPosition(0, _turretPoint);
            LaserLine.SetPosition(1, Player.transform.position);
        }
        else
        {
            //MAKE LINE INVISIBLE!!!
        }
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log(_turretPoint);
    }

    private List<GameObject> GetChildrenComponents()
    {
        List<GameObject> childrenObjects = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "DroneTurretPoint") _turret = transform.GetChild(i).transform;
        }
        return childrenObjects;
    }
}
