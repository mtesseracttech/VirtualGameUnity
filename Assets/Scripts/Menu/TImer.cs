using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TImer : MonoBehaviour
{

    public Text TimerText;
    private float startTime = 90.0f;
    private bool start = false;
    private float _time;
    private float _minutes, _seconds;

    [SerializeField]
    private int _scene;

    // Use this for initialization
    void Start ()
	{
	    start = false;
	    _time = startTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
	   // if (start)
	  //  return;
	    if (_time > 0.0f)
	    {
            _time = startTime - Time.time; // ammount of time since the time has started
            _minutes = (int)_time / 60;
            _seconds = _time % 60;
            TimerText.text = _minutes + ":" + _seconds.ToString("f1");
        }
	    else
	    {
	        CheckGameOver();
	    }
    }

    private void CheckGameOver()
    {
        StartCoroutine(LoadNewScene());
    }


    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(5);
        AsyncOperation async = SceneManager.LoadSceneAsync(_scene);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
