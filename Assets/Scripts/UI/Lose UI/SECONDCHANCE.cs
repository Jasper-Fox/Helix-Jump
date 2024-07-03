using UnityEngine;
using UnityEngine.UI;

public class SECONDCHANCE : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Button _secondChanceButton;
    [SerializeField] private SoundControl _soundControl;

    private void Start()
    {
        _secondChanceButton.onClick.AddListener(Click);  
    }
    
    private void Click()
    {
        _soundControl.Click();
        _player.Reborn();
    }
}
