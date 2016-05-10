using System;
using UnityEngine;

public class ChaseState : AbstractEnemyState
{
    private float _chaseSpeed = 0.01f;
    private float _slerpSpeed = 0.1f;
    private Vector2 _targetXZ;
    private Vector2 _parentXZ;

    public ChaseState(EnemyAgent agent) : base(agent)
    {

    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _agent.NavAgent.destination = _agent.LastSeenTargetPosition;
            _targetXZ = new Vector2(_agent.LastSeenTargetPosition.x, _agent.LastSeenTargetPosition.z);
            _parentXZ = new Vector2(_agent.Parent.transform.position.x, _agent.Parent.transform.position.z);
            _agent.NavAgent.Resume();
            _agent.EnteredNewState = false;
        }

        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof(AttackState));
        }
        else
        {
            _parentXZ.Set(_agent.Parent.transform.position.x, _agent.Parent.transform.position.z);
            _targetXZ.Set(_agent.LastSeenTargetPosition.x, _agent.LastSeenTargetPosition.z);
            Debug.Log("Distance from LastSeenPoint: " + Vector2.Distance(_targetXZ, _parentXZ));
            if(Vector2.Distance(_targetXZ, _parentXZ) < 0.01f)
            {
                _agent.Parent.position = _agent.LastSeenTargetPosition;
                _agent.SetState(typeof(LookoutState));
            }
        }
    }
}


//OLD CODE:
/*
var differenceVector = _agent.LastSeenTargetPosition - _agent.Parent.transform.position;
if (differenceVector.magnitude < 0.1f)
{
    _agent.Parent.position = _agent.LastSeenTargetPosition;
    _agent.SetState(typeof(LookoutState));
}

differenceVector.Normalize();
_agent.Parent.position = _agent.Parent.position + differenceVector * _chaseSpeed;

_agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
_agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
*/
