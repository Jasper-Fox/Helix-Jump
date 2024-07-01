using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Text _text;

    void Update()
    {
        _text.text = _player.NumberOfPassedPlatforms.ToString();
    }
}
