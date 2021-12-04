using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private float _direction;
    [SerializeField] private float speed;
    private bool _vectorX;
    public void SetDirection(float direction, bool vectorX)
    {
        _direction = direction;
        _vectorX = vectorX;
    }

    public void SaySomething()
    {
        Debug.Log("Something!");   
    }

    private void Update()
    {
        if (_direction != 0 && _vectorX)
        {
            var delta = _direction * speed * Time.deltaTime;
            var newX = transform.position.x + delta;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        } else if (_direction != 0 && !_vectorX)
        {
            var delta = _direction * speed * Time.deltaTime;
            var newY = transform.position.y + delta;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
