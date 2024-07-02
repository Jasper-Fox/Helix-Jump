using UnityEngine;
using UnityEngine.UI;

public class LevelPassedText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Game _game;

    private void Start()
    {
        _text.text = $"Level {_game.LevelIndex} Passed";
    }
}