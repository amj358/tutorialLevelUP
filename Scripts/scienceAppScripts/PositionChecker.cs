using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChecker : MonoBehaviour
{
    // hold all of our targets in order
    public GameObject[] targets;

    //ui
    public UI ui;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPositions();
        }
    }

    //checks if targets are in order
    public void CheckPositions()
    {
        //loop through each of our targets
        //get the on screen x position
        //compare it to the previous targert's x position
        //if the current x is less than the previous x, we stop-lost
        //if the loop ends, we win

        bool correctPosition = true;
        float prevX = 0.0f;

        for (int index = 0; index < targets.Length; ++index)
        {
            //get on screen x pos
            float currX = Camera.main.WorldToScreenPoint(targets[index].transform.position).x;

            //skip the first target
            if (index > 0)
            {
                if (currX <= prevX || !targets[index].activeInHierarchy)
                {
                    correctPosition = false;
                    break;
                }
            }

            prevX = currX;

        }



            if (correctPosition)
            {
                


                ui.SetText("You Win!", Color.green, 5.0f);
        }

            else
            {
               
          
                ui.SetText("Incorrect Order", Color.red, 1.0f);


        }
        
    }


}
