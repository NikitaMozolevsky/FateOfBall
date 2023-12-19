using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    // Добавляет 0.05 в каждом кадру, так осуществляется движение
    public float _speed = 0.05f;

    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed;
    }
}
