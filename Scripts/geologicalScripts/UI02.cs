using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI02 : MonoBehaviour
{
    public Text text;
    public Animator textAnimator;

    // instance
    public static UI02 instance;

    void Awake()
    {
        // set the instance to be this script
        instance = this;
    }

    // sets the info text
    public void SetText(string textToDisplay)
    {
        text.gameObject.SetActive(true);
        text.text = textToDisplay;
        textAnimator.Rebind();
        textAnimator.Play("BoundaryTextPopup");
    }
}
