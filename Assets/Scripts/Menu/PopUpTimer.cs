using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUpTimer : MonoBehaviour
{
    public Animator animator;
    private Text popupText;
    private float timeRemaining;
    private ShowThePopUp thePopUp;
    private AnimatorClipInfo[] clipInfo;

    void Start()
    {
        clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        popupText = animator.GetComponent<Text>();
    }
 
    public void UpdateText(string number)
    {
        animator.GetComponent<Text>().text = number;
    }
}
