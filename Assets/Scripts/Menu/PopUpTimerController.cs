using UnityEngine;
using System.Collections;
using Camera = UnityEngine.Camera;

public class PopUpTimerController : MonoBehaviour
{
    private static PopUpTimer popupTextPrefab;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupTextPrefab)
        {
            popupTextPrefab = Resources.Load<PopUpTimer>("Prefabs/PopUpTimeParent");
        }
    }

    public static void CreateFloatingText(string text, Transform location)
    {
        PopUpTimer instance = Instantiate(popupTextPrefab);
        Vector3 screenPosition =
            UnityEngine.Camera.main.WorldToViewportPoint(location.position);

        instance.transform.SetParent(canvas.transform,false);
        instance.transform.position = screenPosition;
        instance.UpdateText(text);
    }
	
}
