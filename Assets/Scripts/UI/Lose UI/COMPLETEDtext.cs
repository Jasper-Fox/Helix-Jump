using UnityEngine;
using UnityEngine.UI;


public class COMPLETEDtext : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _text.text = $"{(int)(_slider.value * 100)}% COMPLETED";
    }
}