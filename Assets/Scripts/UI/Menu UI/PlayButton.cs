using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Button _playButton;
    [SerializeField] private SoundControl _soundControl;

    private void Start()
    {
        _playButton.onClick.AddListener(Click);  
    }

    private void Click()
    {
        _soundControl.Click();
        _game.StartPlay();
    }
}
