using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _defaultIdleTime = 3;
    private float _currentIdleTime = 0;
    private bool _movingLeft = true;

    private void Update()
    {
        if (_movingLeft)
        {
            if (gameObject.transform.position.x >= _leftPoint.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ReverseDirection();
            }
        }
        else
        {
            if (gameObject.transform.position.x <= _rightPoint.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ReverseDirection();
            }
        }
    }

    private void ReverseDirection()
    {
        _currentIdleTime += Time.deltaTime;
        if (_currentIdleTime >= _defaultIdleTime)
        {
            _currentIdleTime = 0;
            _movingLeft = !_movingLeft;
        }
    }

    private void MoveInDirection(int direction)
    {
        gameObject.transform.position =
            new Vector2(gameObject.transform.position.x + Time.deltaTime * direction * _speed,
                gameObject.transform.position.y);
    }
}
