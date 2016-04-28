using UnityEngine;
public class ReturnState : AbstractEnemyState
{
    private float _returnSpeed = 0.01f;
    private readonly Vector3 _originalPoint;
    private readonly Quaternion _originalRotation;

    private float _slerpSpeed = 0.1f;
        
    public ReturnState(EnemyAgent agent, Vector3 originalPoint, Quaternion originalRotation) : base(agent)
    {
        _originalPoint = originalPoint;
        _originalRotation = originalRotation;
        Debug.Log(originalRotation);
    }

    public override void Update()
    {

        Debug.Log(Quaternion.Angle(_originalRotation, _agent.Parent.rotation));
        if (_agent.SeesTarget)
        {
            Debug.Log("Sees the target!");
            _agent.SetState(typeof (ChaseState));
        }
        else
        {
            var differenceVector = _originalPoint - _agent.Parent.position;
            if (differenceVector.magnitude < 0.1f && Quaternion.Angle(_originalRotation, _agent.Parent.rotation) > 0.1f)
            {
                _agent.Parent.position = _originalPoint;
                _agent.SetState(typeof (PatrolState));
            }
            else
            {
                differenceVector.Normalize();
                _agent.Parent.position = _agent.Parent.position + differenceVector * _returnSpeed;
                if (Quaternion.Angle(_originalRotation, _agent.Parent.transform.rotation) > 0.1f)
                {
                    _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
                    _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
                }
            }


        }
    }
}