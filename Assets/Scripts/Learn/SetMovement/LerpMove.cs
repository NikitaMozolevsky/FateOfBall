using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMove : MonoBehaviour
{

    public Transform mTarget;

    Vector3 mStartingPos;
    Vector3 mTargetPos;

    private float elapsedTime;
    public float desiredDuration;
    
    public AnimationCurve curve;

    void Start()
    {
        mStartingPos = transform.position;
        mTargetPos = mTarget.position;
    }

    private void Update()
    { // Не срабатывают сталкивания, все жестко, можно отключать скрипт при коллизии.
        elapsedTime += Time.deltaTime;
        float precentageComplete = elapsedTime / desiredDuration;
        transform.position = Vector3.Lerp
            (mStartingPos, mTargetPos, curve.Evaluate(precentageComplete));
    }
}
