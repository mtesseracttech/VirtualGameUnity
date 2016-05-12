using System.Collections.Generic;
using UnityEngine;

public class PatrolState : AbstractEnemyState
{
    private List<Vector2> _path;
    private Quaternion _originalRotation;
    private Vector2 _currentPos;
    private Vector2 _currentTarget;

    private bool _angleAlignedFirstTime;
    private int _currentPathPoint;
    private float _slerpSpeed = 0.1f;


    public PatrolState(EnemyAgent agent, Quaternion originalRotation, NavigationPath patrolPath = null) : base(agent)
    {
        if (patrolPath != null)
        {
            _path = patrolPath.Path;
            _currentPos = new Vector2(_agent.Parent.position.x, _agent.Parent.position.z);
        }
        _originalRotation = originalRotation;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            if (_path != null)
            {
                _currentTarget = _path[0];
                _agent.NavAgent.SetDestination(new Vector3(_currentTarget.x, _agent.Parent.transform.position.y,
                    _currentTarget.y));
                _agent.NavAgent.Resume();
                _angleAlignedFirstTime = true;
            }
            else
            {
                if (Quaternion.Angle(_agent.Parent.transform.rotation, _originalRotation) > 0.1f) _angleAlignedFirstTime = false;
            }
            _agent.EnteredNewState = false;
            
        }
        if(_path != null)Debug.DrawLine(_agent.Parent.transform.position, new Vector3(_currentTarget.x, _agent.Parent.transform.position.y, _currentTarget.y));

        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof(AttackState));
        }
        else
        {
            if (!_angleAlignedFirstTime)
            {
                //Corrects angle before continuing to patrol
                _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, _originalRotation, _slerpSpeed);
                _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
                if (!(Quaternion.Angle(_agent.Parent.transform.rotation, _originalRotation) > 0.1f)) _angleAlignedFirstTime = true;
            }
            else
            {
                if (_path != null) FollowPath();
                else RotateAroundAxis();
            }
        }
    }

    private void RotateAroundAxis()
    {
        _agent.Parent.transform.Rotate(0, 1.5f, 0);
    }

    private void FollowPath()
    {
        _currentPos.Set(_agent.Parent.transform.position.x , _agent.Parent.transform.position.z); 
        if (Vector2.Distance(_currentPos, _currentTarget) < 2f)
        {
            Debug.Log("REACHED THE POINT");
            _currentTarget = NextPoint();
            _agent.NavAgent.SetDestination(new Vector3(_currentTarget.x, _agent.Parent.transform.position.y, _currentTarget.y));
        }
    }

    private Vector2 NextPoint()
    {
        Debug.Log("Picking Next Point!");
        _currentPathPoint++;
        if (_currentPathPoint >= _path.Count) _currentPathPoint = 0;
        return _path[_currentPathPoint];
    }
}
