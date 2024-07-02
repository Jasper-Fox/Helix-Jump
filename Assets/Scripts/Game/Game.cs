using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private const string LevelIndexKey = "LevelIndex";
    private const string MaxNumberOfPassedPlatformsKey = "maxNumberOfPassedPlatforms";
    
    [SerializeField] private Controls Controls;
    [SerializeField] private GameObject _winUI;
    [SerializeField] private GameObject _loseUI;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private SoundControl _music;
    
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
        get => PlayerPrefs.GetInt(MaxNumberOfPassedPlatformsKey, 0);
        set
        {
            PlayerPrefs.SetInt(MaxNumberOfPassedPlatformsKey, value);
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
        
        _music._lerpMute = true;
    }
    
    public void playerWin()
    {
        if (CurrentState != GameState.Playing) return;
        
        CurrentState = GameState.Won;
        
        Controls.enabled = false;
        
        _winUI.gameObject.SetActive(true);
        
        LevelIndex++;
        
        SetRecord();
        
        _music._lerpMute = true;
    }

    //Перезагружает сцену
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetRecord()
    {
       int numberOfPassedPlatforms = PlayerPrefs.GetInt("numberOfPassedPlatforms", 0);
       
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