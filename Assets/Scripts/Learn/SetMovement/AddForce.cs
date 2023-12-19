using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    // AddForce использует глобальные координаты, поэтоиу forward а не right
    private Rigidbody _rigidbody;
    public float _speed;
    private void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.forward * _speed); // Относительно глобальных координат
        //_rigidbody.AddRelativeForce(Vector3.forward * _speed); // Относительно локальных координат
    }
}
