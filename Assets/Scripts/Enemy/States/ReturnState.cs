using UnityEngine;
public class ReturnState : AbstractEnemyState
{
    private float _returnSpeed = 0.01f;
    private float _slerpSpeed = 0.1f;
    private readonly Vector3 _originalPoint;
    private readonly Quaternion _originalRotation;
        
    public ReturnState(EnemyAgent agent, Vector3 originalPoint, Quaternion originalRotation) : base(agent)
    {
        _originalPoint = originalPoint;
        _originalRotation = originalRotation;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            Debug.Log("Assigned original patrol point");
            _agent.NavAgent.destination = _originalPoint;
            _agent.NavAgent.Resume();
            _agent.EnteredNewState = false;
        }

        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof (AttackState));
        }
        else
        {
            //Debug.Log(Vector3.Distance(_agent.Parent.position, _originalPoint));
            if (Vector3.Distance(_agent.Parent.position, _originalPoint) < 0.1f)
            {
                _agent.Parent.position = _originalPoint;
                _agent.NavAgent.Stop();
                _agent.SetState(typeof(PatrolState));
            }
        }
    }
}

//OLD CODE:
/*
var differenceVector = _originalPoint - _agent.Parent.position;
if (differenceVector.magnitude < _returnSpeed && Quaternion.Angle(_originalRotation, _agent.Parent.rotation) > _slerpSpeed)
{
    _agent.Parent.position = _originalPoint;
    _agent.SetState(typeof (PatrolState));
}
else
{
    differenceVector.Normalize();
    _agent.Parent.position = _agent.Parent.position + differenceVector * _returnSpeed;
    if (Quaternion.Angle(_originalRotation, _agent.Parent.transform.rotation) > _slerpSpeed)
    {
        _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
        _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
    }
}
*/
