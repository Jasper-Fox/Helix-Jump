using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private const string LevelIndexKey = "LevelIndex";
    private const string _numberOfPassedPlatforms = "numberOfPassedPlatforms";
    private const string _maxNumberOfPassedPlatforms = "maxNumberOfPassedPlatforms";
    
    [SerializeField] private Controls Controls;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _menuUI;
    
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
    
    public int MaxNumberOfPassedPlatforms
    {
        get => PlayerPrefs.GetInt(_maxNumberOfPassedPlatforms, 0);
        set
        {
            PlayerPrefs.SetInt(_maxNumberOfPassedPlatforms, value);
            PlayerPrefs.Save();
        } 
    }

    private void Awake()
    {
        StartGame();
    }

    private void StartGame()
    {
        CurrentState = GameState.Start;
        Controls.enabled = false;
        _menuUI.gameObject.SetActive(true);
        _winUI.gameObject.SetActive(false);
        _loseUI.gameObject.SetActive(false);
    }

    public void playerDied()
    {
        //проверка на то что смерть произошла во время игры
        if (CurrentState != GameState.Playing) return; 
        CurrentState = GameState.Loss;
        
        //выключаем код отвечающий за управление
        Controls.enabled = false;
        _loseUI.gameObject.SetActive(true);
        SetRecord();
    }
    
    public void playerWin()
    {
        if (CurrentState != GameState.Playing) return;
        CurrentState = GameState.Won;
        Controls.enabled = false;
        _winUI.gameObject.SetActive(true);
        LevelIndex++;
        SetRecord();
    }

    //Перезагружает сцену
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetRecord()
    {
       int numberOfPassedPlatforms = PlayerPrefs.GetInt(_numberOfPassedPlatforms, 0);
       
       MaxNumberOfPassedPlatforms = Mathf.Max(numberOfPassedPlatforms, MaxNumberOfPassedPlatforms);
    }

    public void StartPlay()
    {
        if (CurrentState != GameState.Start) return;
        CurrentState = GameState.Playing;
        Controls.enabled = true; 
        _menuUI.gameObject.SetActive(false);
    }
}