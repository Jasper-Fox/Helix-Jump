using UnityEngine;
using UnityEngine.UI;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _nextLevelScoreboard;

    void Start()
    {
        Text _nextLevelScoreboardText = _nextLevelScoreboard.GetComponent<Text>();

        _text.text = (_game.LevelIndex + 1).ToString();
        _nextLevelScoreboardText.text = (_game.LevelIndex + 2).ToString();
    }
}