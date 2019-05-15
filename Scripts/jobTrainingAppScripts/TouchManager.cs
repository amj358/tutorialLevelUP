using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<DashboardButton>())
                    {
                        hit.collider.gameObject.GetComponent<DashboardButton>().onHold.Invoke();
                    }
                }
            }
        }
        //.........................................

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<DashboardButton>())
                    {
                        hit.collider.gameObject.GetComponent<DashboardButton>().onHold.Invoke();
                    }
                }
            }
        }

    }


}
