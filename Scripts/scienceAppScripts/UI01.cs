using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI01 : MonoBehaviour
{
    //onScreen Text
    public Text screenText;

    public void SetText(string textToDisplay, Color color, float duration)
    {
        screenText.gameObject.SetActive(true);
        screenText.text = textToDisplay;

        screenText.color = color;

        CancelInvoke("DisableText");
        Invoke("DisableText", duration);

    }

    void DisableText()
    {
        screenText.gameObject.SetActive(false);
    }


    
}
