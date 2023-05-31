using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Mime;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Text TimeCounter;

    private TimeSpan _timePlaying;
    private bool _timerGoing;
    private float _elapsedTime;

    void Start()
    {
        TimeCounter.text = "Time: 00:00:00";
        _timerGoing = false;

    }

    public void BeginTimer()
    {
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
