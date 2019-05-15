using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public GameObject hookConnectionPos;

    void Update()
    {
        Vector3 newPos = hookConnectionPos.transform.localPosition;
        newPos.x = transform.localPosition.x;
        hookConnectionPos.transform.localPosition = newPos;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hookConnectionPos.transform.position);
    }
}
