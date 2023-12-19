using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{ // Куб движется с одинаковой скоростью к цели.

    public float step;
    public Transform target;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards
            (transform.position, target.transform.position, step);
    }
}
