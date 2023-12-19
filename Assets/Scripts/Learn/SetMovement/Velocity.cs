using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{

    public float _spped = 5f;
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * _spped;
    }
}
