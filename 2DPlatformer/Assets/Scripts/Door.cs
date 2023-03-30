using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _previousRoom;
    [SerializeField] private Transform _nextRoom;
    [SerializeField] private CameraController _camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected on the door: " + gameObject.name);
            if (collision.transform.position.x < transform.position.x)
            {
                Debug.Log("Moving camera to the next room");
                _camera.MoveSmoothlyToPoint(_nextRoom);
            }
            else
            {
                Debug.Log("Moving camera to the previous room");
                _camera.MoveSmoothlyToPoint(_previousRoom);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected on the door: " + gameObject.name);
            if (collision.transform.position.x > transform.position.x)
            {
                Debug.Log("Moving camera to the next room");
                _camera.MoveSmoothlyToPoint(_nextRoom);
            }
            else
            {
                Debug.Log("Moving camera to the previous room");
                _camera.MoveSmoothlyToPoint(_previousRoom);
            }
        }
    }

}
