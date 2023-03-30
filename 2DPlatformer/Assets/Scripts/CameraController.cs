using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private float _currentPosX;
    private Vector3 _newPosition;
    private Vector3 _velocity = Vector3.zero;
    private bool _isMovingSmooth = false;
    private bool _isMovingRapidly = false;

    private void Start()
    {
        _newPosition = transform.position;
    }

    private void Update()
    {
        if (_isMovingSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _newPosition, ref _velocity, _speed);
        }
        else if (_isMovingRapidly)
        {
            transform.position = _newPosition;
            _isMovingRapidly = false;
        }
    }


    public void MoveRapidlyToPoint(Transform point)
    {
        Debug.Log("Set new camera rapidly position to: " + point);
        _newPosition = new Vector3(point.position.x, point.position.y, transform.position.z);
        _isMovingSmooth = false;
        _isMovingRapidly = true;
    }

    public void MoveSmoothlyToPoint(Transform point)
    {
        Debug.Log("Set new camera position smoothly to: " + point);
        _newPosition = new Vector3(point.position.x, point.position.y, transform.position.z);
        _isMovingSmooth = true;
        _isMovingRapidly = false;
    }
}
