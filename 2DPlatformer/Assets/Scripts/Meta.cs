using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{
    [SerializeField] private Transform _startingPoint;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private CameraController _camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected, teleporting to starting point: " + _startingPoint.transform);
            collision.transform.position = _startingPoint.position;
            _camera.MoveRapidlyToPoint(_cameraPoint);
        }
    }
}
