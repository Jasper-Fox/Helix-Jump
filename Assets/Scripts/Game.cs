using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Controls Controls;

    public enum State
    {
        Playing,
        Won,
        Loss,
    }
    
    public State CurrentState { get; private set; }

    public void playerDied()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Loss;
        Controls.enabled = false;
    }

    public void playerWin()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Won;
        Controls.enabled = false;
    }
}