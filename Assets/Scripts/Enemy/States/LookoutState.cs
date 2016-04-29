using System;
using UnityEngine;

public class LookoutState : AbstractEnemyState
{
    private float _slerpSpeed = 0.08f;
    private bool _checkedSide1, _checkedSide2;
    private Quaternion _side1, _side2;
    private int _lookScope = 90;
    private int _checkSideFirst;

    public LookoutState(EnemyAgent agent) : base(agent) { }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _side1 = _agent.Parent.transform.rotation*Quaternion.Euler(0, _lookScope, 0);
            _side2 = _agent.Parent.transform.rotation*Quaternion.Euler(0, -_lookScope, 0);

            Debug.Log("Base Direction: " + _agent.Parent.transform.rotation.eulerAngles + " Side 1: " +
                      _side1.eulerAngles + " Side 2: " + _side2.eulerAngles);


            _checkedSide1 = false;
            _checkedSide2 = false;
            System.Random random = new System.Random();
            if (random.Next(-1, 2) < 0) _checkSideFirst = -1; //Turns right first
            else _checkSideFirst = 1; //Turns left first

            _agent.EnteredNewState = false;
        }

        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof (AttackState));
        }
        else
        {
            if (!_checkedSide1)
            {
                Debug.Log("Slerping to Side1");
                /*
                _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, _side1,
                    _slerpSpeed);
                _agent.Parent.transform.rotation =
                    Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
                */
                Slerp(_side1);
                if (Quaternion.Angle(_agent.Parent.rotation, _side1) < 0.1f) _checkedSide1 = true;
            }
            else if (!_checkedSide2)
            {
                Debug.Log("Slerping to Side2");
                Slerp(_side2);
                if (Quaternion.Angle(_agent.Parent.rotation, _side2) < 0.1f) _checkedSide2 = true;
            }
            else _agent.SetState(typeof (ReturnState));
        }
    }

    private void Slerp(Quaternion goal)
    {
        _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, goal, _slerpSpeed);
        _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
    }
}
//OLD CODE
/*
Transform side1t = _agent.Parent.transform;
side1t.Rotate(Vector3.up, -_lookScope);
_side1 = side1t.rotation;
Transform side2t = _agent.Parent.transform;
side2t.Rotate(Vector3.up, _lookScope);
_side2 = side2t.rotation;
*/
//_side1 = Quaternion.RotateTowards(_agent.Parent.transform.rotation, Quaternion.Euler(0, _lookScope, 0), 360);
//_side2 = Quaternion.RotateTowards(_agent.Parent.transform.rotation, Quaternion.Euler(0, -_lookScope, 0), 360);