using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(_game.StartPlay);  
    }
}
