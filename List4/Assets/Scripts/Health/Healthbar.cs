using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health _currentHealth;
    [SerializeField] private Heart[] _hearts;
    private float _currentHealthbarHealth;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _currentHealthbarHealth = _currentHealth.CurrentHealth;
        Debug.Log("Resetting healthbar with value: " + _currentHealthbarHealth);
        foreach (Heart heart in _hearts)
        {
            heart.Reset();
        }
    }
    private void Update()
    {
        float difference = _currentHealthbarHealth - _currentHealth.CurrentHealth;
        if (difference != 0)
        {
            if ((int)_currentHealthbarHealth != _currentHealthbarHealth)
            {
                difference -= 0.5f;
                _currentHealthbarHealth -= 0.5f;
                _hearts[(int)_currentHealthbarHealth].PlayLost();
            }

            while (difference >= 0.99f)
            {
                difference -= 1;
                _currentHealthbarHealth -= 1;
                Debug.Log("Lost heart with value: " + _currentHealthbarHealth + ", and index: " + (int)_currentHealthbarHealth);
                _hearts[(int)_currentHealthbarHealth].PlayHit();
                _hearts[(int)_currentHealthbarHealth].PlayLost();
            }

            if (difference >= 0.49f)
            {
                difference -= 0.5f;
                _currentHealthbarHealth -= 0.5f;
                _hearts[(int)_currentHealthbarHealth].PlayHit();
            }
            Debug.Log("Now current health: " + _currentHealth.CurrentHealth + ", and healthbar current health: " + _currentHealthbarHealth);
        }
    }
}
