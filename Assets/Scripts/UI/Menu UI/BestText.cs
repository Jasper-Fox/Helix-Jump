using UnityEngine;
using UnityEngine.UI;

public class BestText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Game _game;

    private void Awake()
    {
        _text.text = $"best: {_game.MaxNumberOfPassedPlatforms}";
    }
}