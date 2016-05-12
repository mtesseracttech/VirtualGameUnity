using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {
    public Transform Door1;
    public Transform Door2;
    private bool _readyToOpen;
    private bool _readyToClose;
    private bool _open;
    private bool _close;
    private int _counter;
    private int _limit;
    // Use this for initialization
    void Start() {
        _readyToOpen = true;
        _readyToClose = false;
        _open = false;
        _close = false;
        _counter = 0;
        _limit = 15;
    }

    // Update is called once per frame
    void Update() {
        Open();
        Close();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Drone")
        {
            _open = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Drone")
        {
            _close = true;
        }
    }
    void Close()
    {
        if (_readyToClose && _close)
        {
            if (_counter <= _limit)
            {
                Vector3 stepVector = new Vector3(0.1f, 0, 0);
                Door1.localPosition += stepVector;
                Door2.localPosition += stepVector;
                _counter += 1;
                if (_counter > _limit)
                {
                    _readyToOpen = true;
                    _readyToClose = false;
                    _close = false;
                    _counter = 0;
                }
            }
        }
    }
    void Open()
    {
        if (_readyToOpen && _open)
        {
            if (_counter <= _limit)
            {
                Vector3 stepVector = new Vector3(0.1f, 0, 0);
                Door1.localPosition -= stepVector;
                Door2.localPosition -= stepVector;
                _counter += 1;
                if (_counter > _limit)
                {
                    _readyToClose = true;
                    _readyToOpen = false;
                    _open = false;
                    _counter = 0;
                }
            }
        }
    }
}
