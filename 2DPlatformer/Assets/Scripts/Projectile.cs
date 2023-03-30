using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private const float _lifetime = 5f;
    private float _direction = 1;
    private bool _hit = false;
    private float lifeTimeLeft;

    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_hit) return;

        float movementSpeed = _speed * Time.deltaTime * _direction;
        transform.Translate(movementSpeed, 0, 0);
        lifeTimeLeft -= Time.deltaTime;

        if (lifeTimeLeft <= 0)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("WorldMarker"))
        {
            _hit = true;
            _boxCollider2D.enabled = false;
            _animator.SetTrigger("explode");
        }
    }

    public void SetDirection(float direction)
    {
        lifeTimeLeft = _lifetime;
        gameObject.SetActive(true);
        _hit = false;
        _boxCollider2D.enabled = true;
        _direction = direction;

        transform.localScale = new Vector2(direction, 1);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
