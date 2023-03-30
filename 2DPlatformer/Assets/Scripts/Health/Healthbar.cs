using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health _currentHealth;
    [SerializeField] private Image _totalHealthBar;
    [SerializeField] private Image _currentHealthBar;

    private void Start()
    {
        _totalHealthBar.fillAmount = _currentHealth.CurrentHealth / 10;

    }

    private void Update()
    {
        _currentHealthBar.fillAmount = _currentHealth.CurrentHealth / 10;
    }
}
