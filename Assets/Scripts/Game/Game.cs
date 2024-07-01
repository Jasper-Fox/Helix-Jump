using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private const string LevelIndexKey = "LevelIndex";
    
    public Controls Controls;
    
    //текущее состояние которое может изминять только этот код
    public GameState CurrentState { get; private set; }

    /// <summary>
    /// Номер уровня запоминается в плеер префс и сохраняется при перезагрузке сцены. Через гет и сет достаём и назначаем значение
    /// </summary>
    public int LevelIndex
    {
        get => PlayerPrefs.GetInt(LevelIndexKey, 0);
        private set
        {
           PlayerPrefs.SetInt(LevelIndexKey, value);
           PlayerPrefs.Save();
        }
    }
    
    public void playerDied()
    {
        //проверка на то что смерть произошла во время игры
        if (CurrentState != GameState.Playing) return; 
        CurrentState = GameState.Loss;
        
        //выключаем код отвечающий за управление
        Controls.enabled = false;
        ReloadScene();
    }
    
    public void playerWin()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Won;
        Controls.enabled = false;
        LevelIndex++;
        ReloadScene();
    }

    //Перезагружает сцену
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}