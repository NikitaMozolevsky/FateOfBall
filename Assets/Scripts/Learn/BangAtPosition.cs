using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangAtPosition : MonoBehaviour
{

    public Rigidbody targetRigidbody;
    public float forceValue = 500f;

    public void Bang()
    {
        targetRigidbody.AddForceAtPosition(transform.up * forceValue, transform.position);
    }
}
