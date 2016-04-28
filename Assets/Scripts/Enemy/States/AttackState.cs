using UnityEngine;

public class AttackState : AbstractEnemyState
{
    private float _slerpSpeed = 0.1f;

    public AttackState(EnemyAgent agent) : base(agent)
    {

    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            //_agent.ForceVision();
            _agent.EnteredNewState = false;
        }
        if (_agent.SeesTarget)
        {
            var differenceVector = _agent.Target.position - _agent.Parent.position;
            _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
            _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
            LineOfAimHandler();
        }
        else
        {
            _agent.SetState(typeof(ChaseState));
        }
    }

    void LineOfAimHandler()
    {

        RaycastHit hit;
        if (Physics.Raycast(_agent.Parent.transform.position, _agent.Parent.transform.forward, out hit))
        {
            Debug.DrawLine(_agent.Parent.transform.position, _agent.Parent.transform.position + _agent.Parent.transform.forward, Color.green);
            Debug.Log("Raycasting!");
            if (hit.collider.gameObject.tag == "Player") Debug.Log("hitting the player!"); //Checks if anything is between the Target and the Parent
        }
    }
}
