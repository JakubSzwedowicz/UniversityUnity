
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip _checkpointClip;
    private Transform _currentCheckpoint;
    private Health _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = _currentCheckpoint.position;
        _playerHealth.Respawn();
    }
}
