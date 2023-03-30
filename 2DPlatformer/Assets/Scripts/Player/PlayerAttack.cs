using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float AttackCooldown = 0.5f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject[] _fireballs;
    [SerializeField] private AudioClip _fireballClip;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private float _cooldownTimer = 0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && _playerMovement.canAttack() && !OnCooldown())
        {
            SoundManager.Instance.PlaySound(_fireballClip);
            _animator.SetTrigger("attack");
            _cooldownTimer = AttackCooldown;

            _fireballs[FindFireball()].transform.position = _firePoint.position;
            _fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < _fireballs.Length; i++)
        {
            if (!_fireballs[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private bool OnCooldown()
    {
        return _cooldownTimer > 0;
    }
}
