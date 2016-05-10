using UnityEngine;

public class PatrolState : AbstractEnemyState
{
    private Quaternion _originalRotation;
    private bool _angleAlignedFirstTime;
    private float _slerpSpeed = 0.1f;

    public PatrolState(EnemyAgent agent, Quaternion originalRotation) : base(agent)
    {
        _originalRotation = originalRotation;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            if(Quaternion.Angle(_agent.Parent.transform.rotation, _originalRotation) > 0.1f) _angleAlignedFirstTime = false;
            _agent.EnteredNewState = false;
            
        }

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
                _agent.Parent.transform.Rotate(0, 1.5f, 0);
                _agent.Parent.transform.Translate(0, 0, 0.01f);
            }
        }
    }
}
