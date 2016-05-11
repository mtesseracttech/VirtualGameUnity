using System;
using System.ComponentModel;
using UnityEngine;

public class AttackState : AbstractEnemyState
{
    private float _slerpSpeed = 0.1f;
    //private GameObject _gun;
    //private GameObject _bulletPrefab;
    private Vector3 _actualAim;
    private System.Random random;
    private int _aimDistortion = 10; //The lower, the less distortion in degrees
    private float _aimSteadiness = 0.1f; // the higher, the more inaccurate
    private int _acceptableShotRange = 20; //Max distortion (in deg) in which enemy will still shoot
    private bool _acceptableShot;
    private bool _countFireDelta;
    private float _fireDelta;
    private Transform _turret;
    private Vector3 _turretVector;
    private GameObject _muzzleFlash;
    private GameObject _bloodParticles;
    private GameObject _impactParticles;

    public AttackState(EnemyAgent agent, GameObject muzzleFlash, GameObject bloodParticles, GameObject impactParticles, GameObject turretPoint) : base(agent)
    {
        random = new System.Random();
        _muzzleFlash = muzzleFlash;
        //_gun = gun;
        //_bulletPrefab = bulletPrefab;
        _bloodParticles = bloodParticles;
        _impactParticles = impactParticles;
        _turret = turretPoint.transform;
        _turretVector = _turret.position;
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            _agent.EnteredNewState = false;
        }
        if (_agent.SeesTarget)
        {
            var differenceVector = _agent.Target.position - _agent.Parent.position;
            _agent.Parent.transform.rotation = Quaternion.Slerp(_agent.Parent.transform.rotation, Quaternion.LookRotation(differenceVector, Vector3.up), _slerpSpeed);
            _agent.Parent.transform.rotation = Quaternion.Euler(new Vector3(0f, _agent.Parent.transform.rotation.eulerAngles.y, 0f));
            LineOfAimHandler();
            Debug.Log("I should still be seeing the player!");
        }
        else
        {
            _agent.SetState(typeof(ChaseState));
        }
    }

    void LineOfAimHandler()
    {
        _turretVector = _turret.position;
        Vector3 differenceVector = _agent.Target.transform.position - _turretVector; //Vector to get length and height from
        Vector3 customVector = _agent.Parent.transform.forward; //Vector to use as a guide while aiming, has randomness
        customVector.Normalize();
        customVector *= differenceVector.magnitude;
        customVector.y = differenceVector.y;
        customVector = Quaternion.Euler(0, random.Next(-_aimDistortion, _aimDistortion + 1), random.Next(-_aimDistortion, _aimDistortion + 1)) * customVector;
        _actualAim = Vector3.Slerp(_actualAim, customVector, _aimSteadiness); //Vector that the actual shot is cast from, slerps between different customVec pos's
        _actualAim.Normalize();
        _actualAim *= differenceVector.magnitude;

        var targetAngle = Vector3.Angle(_actualAim, differenceVector);
        if (targetAngle > _acceptableShotRange)
        {
            //Debug.DrawLine(_agent.Parent.transform.position, _agent.Parent.transform.position + _actualAim, Color.red);
            Debug.DrawLine(_turretVector, _turretVector + _actualAim, Color.red);
            //Sees if Target is in acceptable range, if not, no ray is cast
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(_turretVector, _actualAim, out hit))
            {
                Debug.Log("Aiming at player!");
                Shoot(hit);
            }
        }
        
    }

    void Shoot(RaycastHit hit)
    {
        Debug.Log("Shoot Activated");
        if (!_countFireDelta) _countFireDelta = true;
        if (_fireDelta == 0)
        {
            Debug.Log("Shooting!");
            _agent.CreateParticlesRotated(_muzzleFlash, _turretVector, Quaternion.LookRotation(_actualAim, Vector3.up));
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _agent.CreateParticles(_bloodParticles, hit.point);
                Debug.Log("Player was hit!");
                Debug.DrawLine(_turretVector, _turretVector + _actualAim, Color.green);
            }
            else
            {
                _agent.CreateParticles(_impactParticles, hit.point);
                Debug.Log("Something else was hit!");
                Debug.DrawLine(_turretVector, _turretVector + _actualAim, Color.magenta);
            }
        }
        else
        {
            Debug.Log("Waiting for next shot");
            Debug.DrawLine(_turretVector, _turretVector + _actualAim, Color.white);
        }
        if (_countFireDelta)
        {
            _fireDelta += Time.deltaTime;
        }
        if (_fireDelta >= 1f /*_gun.GetComponent<GunScript>().FireDelay() */)
        {
            _fireDelta = 0;
            _countFireDelta = false;
        }
    }
}

