using UnityEngine;

public class AttackState : AbstractEnemyState
{
    private float _slerpSpeed = 0.1f;

    public AttackState(EnemyAgent agent) : base(agent) {}

    public override void Update()
    {
        if (_agent.SeesTarget)
        {
            var differenceVector = _agent.Target.position - _agent.Parent.position;
            _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
            _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
        }
        else
        {
            _agent.SetState(typeof(ChaseState));
        }
    }
}
