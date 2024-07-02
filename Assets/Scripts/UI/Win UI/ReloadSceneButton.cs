using UnityEngine;
using UnityEngine.UI;

public class ReloadSceneButton : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Button _reloadSceneButton;

    private void Start()
    {
        _reloadSceneButton.onClick.AddListener(_game.ReloadScene);
    }
}
