using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TimerController TimerController;
    public SoundManager SoundManager;

    private void Start()
    {
        PlayGame(0);
    }

    public void PlayGame(int LevelNumber)
    {
        TimerController.BeginTimer();
    }

    public void StopGame()
    {
        TimerController.EndTimer();
    }
}
