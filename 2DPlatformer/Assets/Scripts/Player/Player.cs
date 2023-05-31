
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip _checkpointClip;
    [SerializeField] private Healthbar _healthBar;
    private Transform _currentCheckpoint;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = _currentCheckpoint.position;
        _health.Respawn();
        _healthBar.Reset();

        Camera.main.GetComponent<CameraController>().MoveRapidlyToPoint(_currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            _currentCheckpoint = other.transform;
            SoundManager.Instance.PlaySound(_checkpointClip);
            other.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
