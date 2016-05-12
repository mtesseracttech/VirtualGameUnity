
using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject Player;

    // Variables used for camera shake effect when the player jumps
    public float HorizontalShakeValue = 0.075f;
    public float VerticalViewBobbingValue = 0.0075f;
    public int JumpLimit = 5;
    private bool _shake;
    private bool _jumpUp;
    private bool _jumped;
    private int _jumpCounter;

    // Variables used for View Bobbing
    private float _cameraY;
    private bool _bob;
    private bool _bobUp;
    private int _bobLimit;
    private int _bobCounter;

    // Use this for initialization
    void Start()
    {
        _cameraY = gameObject.transform.localPosition.y;

        _bobLimit = 20;
        _bobCounter = 0;
        _jumpCounter = 0;

        _shake = false;
        _jumpUp = false;
        _jumped = false;
        _bob = false;
        _bobUp = false; 
    }

    // Update is called once per frame
    void Update()
    {
        OnJumpCameraShake();
        ViewBobbing(); 
    }
    void ViewBobbing()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            _bob = true;
        Vector3 stepVector = new Vector3(0, VerticalViewBobbingValue, 0);
        if (_bob)
        {
            if (!_bobUp)
            {
                gameObject.transform.localPosition -= stepVector;
                _bobCounter += 1;
                if (_bobCounter >= _bobLimit)
                {
                    _bobUp = !_bobUp;
                    _bobCounter = 0;
                }
            }
            if (_bobUp)
            {
                gameObject.transform.localPosition += stepVector;
                _bobCounter += 1;
                if (_bobCounter >= _bobLimit)
                {
                    _bobUp = !_bobUp;
                    _bobCounter = 0;
                }
            }
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            _bob = false;
            _bobUp = false;
            gameObject.transform.localPosition = new Vector3(transform.localPosition.x, _cameraY, transform.localPosition.z);
            _bobCounter = 0;
        }
    }
    void OnJumpCameraShake()
    {
        if (Player.GetComponent<PlayerControls2>().Grounded == false)
        {
            _jumped = true;
        }
        if (Player.GetComponent<PlayerControls2>().Grounded && _jumped)
        {
            _shake = true;
            _jumped = false;
        }
        if (_shake)
        {
            Vector3 stepVector = new Vector3(0, HorizontalShakeValue, 0);
            if (!_jumpUp && _jumpCounter < JumpLimit)
            {
                gameObject.transform.localPosition -= stepVector;
                _jumpCounter += 1;
                if (_jumpCounter >= JumpLimit)
                {
                    _jumpUp = true;
                    _jumpCounter = 0;
                }
            }
            else if (_jumpUp && _jumpCounter < JumpLimit)
            {
                gameObject.transform.localPosition += stepVector;
                _jumpCounter += 1;
                if (_jumpCounter >= JumpLimit)
                {
                    _jumpUp = false;
                    _shake = false;
                    _jumpCounter = 0;
                }
            }
        }
    }
}

