using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Mime;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text TimeCounter;

    public TimeSpan _timePlaying;
    public bool _timerGoing;
    public float _elapsedTime;

    private void Awake()
    {
        TimeCounter.text = "Time: 00:00:00";
        _timerGoing = false;
    }
    public void BeginTimer()
    {
        TimeCounter.text = "Time: 00:00:00";
        _timerGoing = true;
        _elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        _timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (_timerGoing)
        {
            _elapsedTime += Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_elapsedTime);
            string timePlayingStr = "Time: " + _timePlaying.ToString("mm':'ss':'ff");
            TimeCounter.text = timePlayingStr;
            yield return null;
        }
    }
}
