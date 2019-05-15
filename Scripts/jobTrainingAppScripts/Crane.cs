using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    // speeds
    public float turnSpeed;             // rate at which the crane can rotate on the Y axis
    public float hookVerticalSpeed;     // rate at which the hook can be raised and lowered
    public float hookHorizontalSpeed;   // rate at which the hook can be moved horizontally along the crane

    // hook
    public float hookRaiseLimit;        // the highest the hook can be raised
    public float hookLowerLimit;        // the lowest the hook can be lowered
    public float hookForwardsLimit;     // the furthest forward the hook can be moved
    public float hookBackwardsLimit;    // the furthest backwards the hook can be moved

    // components
    public GameObject craneTop;         // top of the crane which rotates
    public GameObject hook;             // the hook object

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            Turn(1);
        if (Input.GetKey(KeyCode.Q))
            Turn(-1);
        if (Input.GetKey(KeyCode.W))
            HookVertical(1);
        if (Input.GetKey(KeyCode.S))
            HookVertical(-1);
        if (Input.GetKey(KeyCode.D))
            HookHorizontal(-1);
        if (Input.GetKey(KeyCode.A))
            HookHorizontal(1);
    }

    // rotates the crane along the Y axis
    public void Turn(int dir)
    {
        craneTop.transform.Rotate(Vector3.up, dir * turnSpeed * Time.deltaTime);
    }

    // move the hook forwards / back
    public void HookHorizontal(int dir)
    {
        MoveHook(new Vector2(dir, 0));
    }

    // move the hook up / down
    public void HookVertical(int dir)
    {
        MoveHook(new Vector2(0, dir));
    }

    // moves the hook
    void MoveHook(Vector2 dir)
    {
        hook.transform.localPosition += new Vector3(dir.x * hookHorizontalSpeed, dir.y * hookVerticalSpeed, 0) * Time.deltaTime;

        // clamp hook's position
        Vector3 clampedPos = hook.transform.localPosition;

        clampedPos.x = Mathf.Clamp(clampedPos.x, hookForwardsLimit, hookBackwardsLimit);
        clampedPos.y = Mathf.Clamp(clampedPos.y, hookLowerLimit, hookRaiseLimit);

        hook.transform.localPosition = clampedPos;
    }
}
