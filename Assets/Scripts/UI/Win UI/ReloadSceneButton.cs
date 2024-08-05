using UnityEngine;
using UnityEngine.UI;

public class ReloadSceneButton : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Button _reloadSceneButton;
    [SerializeField] private bool _isLoseReloadSceneButton;

    private void Start()
    {
        _reloadSceneButton.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (_isLoseReloadSceneButton)
            PlayerPrefs.SetInt("numberOfPassedPlatforms", 0);
        _game.ReloadScene();
    }
}