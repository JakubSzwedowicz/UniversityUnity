using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _HitHeart;
    private Animator _animator;
    private Image _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<Image>();
    }

    public void Reset()
    {
        _animator.ResetTrigger("hit");
        _animator.ResetTrigger("lost");
        SetFull();
        _animator.Play("Idle");
    }

    public void PlayHit()
    {
        _animator.SetTrigger("hit");
    }

    public void PlayLost()
    {
        _animator.SetTrigger("lost");
    }


    private void SetLost()
    {
        _spriteRenderer.sprite = _fullHeart;
        _spriteRenderer.color = Color.black;
    }

    private void SetFull()
    {
        _spriteRenderer.sprite = _fullHeart;
        _spriteRenderer.color = Color.white;
    }

    private void SetHit()
    {
        _spriteRenderer.sprite = _HitHeart;
        _spriteRenderer.color = Color.red;
    }
}
