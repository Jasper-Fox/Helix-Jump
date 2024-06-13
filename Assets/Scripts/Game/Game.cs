using Enums;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Controls Controls;
    
    //текущее состояние которое может изминять только этот код
    public GameState CurrentState { get; private set; }

    public void playerDied()
    {
        //проверка на то что смерть произошла во время игры
        if (CurrentState != GameState.Playing) return; 
        CurrentState = GameState.Loss;
        
        //выключаем код отвечающий за управление
        Controls.enabled = false; 
    }

    public void playerWin()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Won;
        Controls.enabled = false;
    }
}