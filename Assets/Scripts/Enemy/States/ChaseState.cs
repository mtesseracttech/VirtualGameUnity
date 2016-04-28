using UnityEngine;

public class ChaseState : AbstractEnemyState
{
    private float _chaseSpeed = 0.01f;
    private float _slerpSpeed = 0.1f;

    public ChaseState(EnemyAgent agent) : base(agent)
    {
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _agent.NavAgent.destination = _agent.LastSeenTargetPosition;
            _agent.NavAgent.Resume();
            _agent.EnteredNewState = false;
        }
        _agent.ForceVision();
        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof(AttackState));
        }
        else
        {
            //Debug.Log(Vector3.Distance(_agent.Parent.transform.position, _agent.LastSeenTargetPosition));
            if (Vector3.Distance(_agent.Parent.transform.position, _agent.LastSeenTargetPosition) < 0.1f) //Kind of a hack because Unity is being weird
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
