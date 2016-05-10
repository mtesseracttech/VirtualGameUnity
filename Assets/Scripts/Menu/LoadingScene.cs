using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private bool _loadScene;
    [SerializeField]
    private int _scene;
    [SerializeField]
    private Text _loadingText;

    void Update()
    {
        if (!_loadScene)
        {
            _loadScene = true;
            _loadingText.text = "Loading...";
            StartCoroutine(LoadNewScene());
        }

        if (_loadScene)
        {
            _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
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
