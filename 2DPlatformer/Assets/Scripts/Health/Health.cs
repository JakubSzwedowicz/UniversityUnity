using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _startingHealth = 3;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _hurtClip;
    [SerializeField] private Behaviour[] _behaviours;
    public float CurrentHealth { get; private set; }
    private Animator _animator;
    private bool _isDead = false;

    private void Awake()
    {
        CurrentHealth = _startingHealth;
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Math.Clamp(CurrentHealth - damage, 0, _startingHealth);

        if (CurrentHealth > 0)
        {
            _animator.SetTrigger("hurt");
        }
        else
        {
            if (!_isDead)
            {
                Die();
            }
        }
    }

    public void AddHealth(float health)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + health, 0, _startingHealth);
    }

    public void Respawn()
    {
        _isDead = false;
        _animator.ResetTrigger("die");
        _animator.Play("Idle");
        AddHealth(_startingHealth);
        foreach (Behaviour behaviour in _behaviours)
        {
            behaviour.enabled = true;
        }
    }

    private void Die()
    {
        Debug.Log("Component is dead");
        _isDead = true;
        _animator.SetBool("grounded", true);
        _animator.SetTrigger("die");
        foreach (Behaviour behaviour in _behaviours)
        {
            behaviour.enabled = false;
        }
        /* Destroy(gameObject);*/
    }

    private void PlayDeathSound()
    {
        SoundManager.Instance.PlaySound(_deathClip);
    }

    private void PlayHurtSound()
    {
        SoundManager.Instance.PlaySound(_hurtClip);
    }
}
