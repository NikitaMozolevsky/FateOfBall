using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransform : MonoBehaviour
{

    public float speed = 1f;
    private Vector3 newPosition;
    private bool isMoving = false;

    private void Update()
    {
        if (Time.time > 1f)
        {
            Move();    
        }
    }

    private void Move()
    {
        newPosition = new Vector3(transform.position.x,
            transform.position.y, transform.position.z + speed * Time.deltaTime);
        transform.position = newPosition;
    }
    
    private IEnumerator MoveToTarget(Vector3 target, float speed)
    {
        isMoving = true;

        float elapsedTime = 0f;

        Vector3 startingPos = transform.position;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startingPos, target, elapsedTime);
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }

        // Гарантируем, что объект точно достигнет целевой точки
        transform.position = target;

        isMoving = false;
    }
}
