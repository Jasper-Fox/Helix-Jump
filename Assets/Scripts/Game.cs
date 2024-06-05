using UnityEngine;

public class Game : MonoBehaviour
{
    public Controls Controls;

    public enum State //список состояний
    {
        Playing,
        Won,
        Loss,
    }
    
    public State CurrentState { get; private set; } //текущее состояние которое может изминять только этот код

    public void playerDied()
    {
        if (CurrentState != State.Playing) return; //проверка на то что смерть произошла во время игры
        CurrentState = State.Loss;
        Controls.enabled = false; //выключаем код отвечающий за управление
    }

    public void playerWin()
    {
        if (CurrentState != State.Playing) return;
        CurrentState = State.Won;
        Controls.enabled = false;
    }
}