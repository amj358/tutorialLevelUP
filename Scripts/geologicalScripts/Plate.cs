using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlateMovement
{
    Unassigned,     // no assigned movement
    TransformForward,
    TransformBack,
    Divergent,
    ConvergentUp,
    ConvergentDown
}

public class Plate : MonoBehaviour
{
    public GameObject plateObject;  // plate game object
    public PlateMovement assignedMovement;  // plate's assigned movement
    public GameObject arrowVisual;  // plate's arrow visual
    public Animator arrowAnimator;  // plate's arrow animator component
}
