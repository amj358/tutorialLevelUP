using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager_02 : MonoBehaviour
{
    // plate we're currently touching
    public Plate touchingPlate;

    // world position where the touch began
    private Vector3 touchStartPos;

    // time that the player last tapped on screen
    private float lastTapTime;

    // max time to double tap
    private float doubleTapMaxTime = 0.2f;

    // min distance you have to drag to register a direction
    private float minDragDistance = 0.3f;

    void Update()
    {
        // are there any touches on screen?
        if (Input.touchCount > 0)
        {
            // did the touch Begin on this frame?
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                // check double tap set our touch
                SetStartTouch(Input.touches[0].position);
                DoubleTapCheck();
            }
            // did the touch End on this frame?
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                // calculate the drag direction
                CalculateDragDirectionOnPlate(Input.touches[0].position);
            }
        }

        // TESTING PURPOSES - check for mouse inputs
        if (Input.GetMouseButtonDown(0))
        {
            // check double tap set our touch
            SetStartTouch(Input.mousePosition);
            DoubleTapCheck();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CalculateDragDirectionOnPlate(Input.mousePosition);
        }
    }

    // sets the start touch to be a world space position
    void SetStartTouch(Vector3 touchPosition)
    {
        touchingPlate = null;

        // create a ray from the camera to touch pos
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // shoot the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // did we hit anything?
            if (hit.collider != null)
            {
                // set the touchStart pos
                touchStartPos = hit.point;

                // get our tectonic plate
                touchingPlate = hit.collider.gameObject.GetComponent<Plate>();
            }
        }
    }

    // check for a double tap and reset plates
    void DoubleTapCheck()
    {
        if (Time.time - lastTapTime <= doubleTapMaxTime)
        {
            // reset plates
            PlateManager.instance.DoubleTapResetPlates();
        }

        lastTapTime = Time.time;
    }

    // called after the dragged touch has been released
    void CalculateDragDirectionOnPlate(Vector3 touchEnd)
    {
        // return if we have no touching plate
        if (!touchingPlate)
            return;

        // create world pos for touch end
        Vector3 touchEndWorldPos = Vector3.zero;

        // create a ray
        Ray ray = Camera.main.ScreenPointToRay(touchEnd);

        // plane raycast
        Plane rayPlane = new Plane(Vector3.up, new Vector3(0, touchStartPos.y, 0));

        float enter = 0;

        // send the raycast
        if (rayPlane.Raycast(ray, out enter))
        {
            touchEndWorldPos = ray.GetPoint(enter);
        }

        // was this not a drag but a tap? If so, unassign plate
        if (Vector3.Distance(touchStartPos, touchEndWorldPos) < minDragDistance)
        {
            // unassign the selected plate
            PlateManager.instance.UnassignPlate(touchingPlate);
            return;
        }

        // get direction between 2 points and round it
        Vector3 dir = Vector3.Normalize(touchEndWorldPos - touchStartPos);
        dir = new Vector3(Mathf.Round(dir.x), 0, Mathf.Round(dir.z));

        Vector3 plateDir = new Vector3(dir.x, 0, dir.z);

        // assign the plate movement
        PlateManager.instance.AssignPlateMovement(touchingPlate, -plateDir);

        //Debug.Log(plateDir);
    }
}