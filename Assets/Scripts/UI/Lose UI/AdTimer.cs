using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdTimer : MonoBehaviour
{
    [Min(0)] [SerializeField] private float _timerSpeed;

    [SerializeField] private Image _timeLine;
    [SerializeField] private GameObject _adButton;
    [SerializeField] private SoundControl _music;

    void Update()
    {
        _timeLine.fillAmount -= Time.deltaTime * _timerSpeed;

        if (_timeLine.fillAmount <= 0)
            HideAdButtonAndMute();
    }

    public void HideAdButtonAndMute()
    {
        _adButton.SetActive(false);
            
        _music._lerpMute = true;
    }
}