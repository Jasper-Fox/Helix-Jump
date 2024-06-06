using Enums;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Controls Controls;
    
    public GameState CurrentState { get; private set; } //текущее состояние которое может изминять только этот код

    public void playerDied()
    {
        if (CurrentState != GameState.Playing) return; //проверка на то что смерть произошла во время игры
        CurrentState = GameState.Loss;
        Controls.enabled = false; //выключаем код отвечающий за управление
    }

    public void playerWin()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Won;
        Controls.enabled = false;
    }
}