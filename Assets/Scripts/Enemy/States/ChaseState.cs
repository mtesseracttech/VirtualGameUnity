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
        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof(AttackState));
        }
        else
        {
            var differenceVector = _agent.LastSeenTargetPosition - _agent.Parent.transform.position;
            if (differenceVector.magnitude < 0.1f)
            {
                _agent.Parent.position = _agent.LastSeenTargetPosition;
                _agent.SetState(typeof(ReturnState));
            }
            differenceVector.Normalize();
            _agent.Parent.position = _agent.Parent.position + differenceVector * _chaseSpeed;
            _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
            _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
        }
    }
}