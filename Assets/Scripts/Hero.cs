using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private float _direction;
    [SerializeField] private float speed;
    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    public void SaySomething()
    {
        Debug.Log("Something!");   
    }

    private void Update()
    {
        if (_direction != 0)
        {
            var delta = _direction * speed * Time.deltaTime;
            var newX = transform.position.x + delta;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}
