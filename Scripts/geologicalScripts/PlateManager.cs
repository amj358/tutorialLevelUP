using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class PlateManager : MonoBehaviour
{
    // plates
    public Plate leftPlate;
    public Plate rightPlate;

    // states
    public bool currentlyAnimating; // are the plates currently animating?
    
    // timeline playable assets
    public PlayableAsset transformForwardsBackAnim;
    public PlayableAsset transformBackForwardsAnim;
    public PlayableAsset divergentAnim;
    public PlayableAsset convergentUpDownAnim;
    public PlayableAsset convergentDownUpAnim;
    
    // component
    public PlayableDirector director;       // director component

    // instance
    public static PlateManager instance;

    void Awake()
    {
        // set instance to this object
        instance = this;
    }

    // called after the user drags a direction for the plate to move in
    public void AssignPlateMovement(Plate plate, Vector3 moveDirection)
    {
        // return if we're currently animating
        if (currentlyAnimating)
            return;

        // invert move direction X if the plate is the left one
        if (plate == leftPlate)
            moveDirection.x = -moveDirection.x;

        // did the user swipe FORWARDS?
        if (moveDirection == Vector3.forward)
            plate.assignedMovement = PlateMovement.TransformForward;
        // did the user swipe BACK?
        else if (moveDirection == Vector3.back)
            plate.assignedMovement = PlateMovement.TransformBack;
        // did the user swipe away from the center?
        else if (moveDirection == Vector3.right)
            plate.assignedMovement = PlateMovement.Divergent;
        // did the user swipe towards the center?
        else if (moveDirection == Vector3.left)
        {
            // get the other plate
            Plate otherPlate = plate == leftPlate ? rightPlate : leftPlate;

            // if the other plate converges down
            if (otherPlate.assignedMovement == PlateMovement.ConvergentDown)
                plate.assignedMovement = PlateMovement.ConvergentUp;
            else
                plate.assignedMovement = PlateMovement.ConvergentDown;
        }

        // set arrow visual
        plate.arrowVisual.SetActive(true);

        // rotate arrow depending on assigned movement
        switch (plate.assignedMovement)
        {
            case PlateMovement.TransformForward:
                plate.arrowVisual.transform.eulerAngles = new Vector3(0, 180, 0);
                break;
            case PlateMovement.TransformBack:
                plate.arrowVisual.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case PlateMovement.Divergent:
                plate.arrowVisual.transform.eulerAngles = new Vector3(0, plate == leftPlate ? 90 : -90, 0);
                break;
            case PlateMovement.ConvergentUp:
                plate.arrowVisual.transform.eulerAngles = new Vector3(0, plate == leftPlate ? -90 : 90, 0);
                break;
            case PlateMovement.ConvergentDown:
                plate.arrowVisual.transform.eulerAngles = new Vector3(0, plate == leftPlate ? -90 : 90, 0);
                break;
        }

        // do both plates have an assigned movement?
        if (leftPlate.assignedMovement != PlateMovement.Unassigned && rightPlate.assignedMovement != PlateMovement.Unassigned)
        {
            // are the 2 plates compatible with each other?
            if (PlatesAreCompatible())
            {
                // play the animation
                Invoke("PlayAnimation", 0.5f);
            }
            else
            {
                // cancel
                Invoke("OnAnimationEnd", 0.35f);
            }
        }
    }

    // unassign a plate, set movement to Unassign
    public void UnassignPlate(Plate plate)
    {
        Debug.Log(plate.name);
        plate.assignedMovement = PlateMovement.Unassigned;
        StartCoroutine(DeactivateArrowVisual(plate));
    }

    // plays the assigned plate animation
    void PlayAnimation()
    {
        // disable arrows
        StartCoroutine(DeactivateArrowVisual(leftPlate));
        StartCoroutine(DeactivateArrowVisual(rightPlate));

        // assign the corresponding timeline to the director
        switch (leftPlate.assignedMovement)
        {
            case PlateMovement.TransformForward:
                {
                    director.playableAsset = transformForwardsBackAnim;
                    UI02.instance.SetText("Transform Boundary");
                    break;
                }
            case PlateMovement.TransformBack:
                {
                    director.playableAsset = transformBackForwardsAnim;
                    UI02.instance.SetText("Transform Boundary");
                    break;
                }
            case PlateMovement.Divergent:
                {
                    director.playableAsset = divergentAnim;
                    UI02.instance.SetText("Divergent Boundary");
                    break;
                }
            case PlateMovement.ConvergentUp:
                {
                    director.playableAsset = convergentUpDownAnim;
                    UI02.instance.SetText("Convergent Boundary");
                    break;
                }
            case PlateMovement.ConvergentDown:
                {
                    director.playableAsset = convergentDownUpAnim;
                    UI02.instance.SetText("Convergent Boundary");
                    break;
                }
        }

        // play animation
        currentlyAnimating = true;
        director.Play();
        Invoke("OnAnimationEnd", (float)director.playableAsset.duration);
    }

    // deactiavte arrow with animation
    IEnumerator DeactivateArrowVisual(Plate plate)
    {
        plate.arrowAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(0.3f);
        plate.arrowVisual.SetActive(false);
    }

    // called after the plate animation has finished
    void OnAnimationEnd()
    {
        currentlyAnimating = false;
        UnassignPlate(leftPlate);
        UnassignPlate(rightPlate);
    }

    // returns true if both plates are compatible with each other
    bool PlatesAreCompatible()
    {
        // return FALSE if...

        // plate are both transforming forward
        if (leftPlate.assignedMovement == PlateMovement.TransformForward && rightPlate.assignedMovement == PlateMovement.TransformForward)
            return false;
        // plate are both transforming backwards
        else if (leftPlate.assignedMovement == PlateMovement.TransformBack && rightPlate.assignedMovement == PlateMovement.TransformBack)
            return false;
        // left plate is diverging but the right plate is not
        else if (leftPlate.assignedMovement == PlateMovement.Divergent && rightPlate.assignedMovement != PlateMovement.Divergent)
            return false;
        // right plate is diverging but the left plate is not
        else if (rightPlate.assignedMovement == PlateMovement.Divergent && leftPlate.assignedMovement != PlateMovement.Divergent)
            return false;
        // left plate is converging up but right is not converging down
        else if (leftPlate.assignedMovement == PlateMovement.ConvergentUp && rightPlate.assignedMovement != PlateMovement.ConvergentDown)
            return false;
        // right plate is converging up but left is not converging down
        else if (rightPlate.assignedMovement == PlateMovement.ConvergentUp && leftPlate.assignedMovement != PlateMovement.ConvergentDown)
            return false;
        else
            return true;
    }

    // resets plates after double tap
    public void DoubleTapResetPlates()
    {
        director.Stop();
        director.playableAsset = null;
        currentlyAnimating = false;
        UI02.instance.text.gameObject.SetActive(false);
        GameObject.Find("FrictionParticle").SetActive(false);
        UnassignPlate(leftPlate);
        UnassignPlate(rightPlate);
    }
}
