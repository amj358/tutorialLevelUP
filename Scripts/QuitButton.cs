using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public void Done ()
    {
        Debug.Log("he has quit the game");


        Application.Quit();
    }
}
