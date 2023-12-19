using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateMovement : MonoBehaviour
{
    // Аналогично SetPosition
    public float _speed = 0.05f;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed);
    }
}
