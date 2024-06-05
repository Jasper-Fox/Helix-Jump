using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    public Player Player;
    public Transform FinishPlatform;
    public Slider Slider;

    private float _startPlayerPositionY;
    private float _playerPositionY;
    private float _finishPositionY;
    private float _minPlayerPositionY;
    
    void Start()
    {
        SetStartAndFin();
    }

    void Update()
    {
        SliderValue();
    }

    private void SliderValue()
    {
        _playerPositionY = Player.transform.position.y;
        _minPlayerPositionY = Mathf.Min(_playerPositionY ,  _minPlayerPositionY);
        float t = Mathf.InverseLerp(_startPlayerPositionY, _finishPositionY + 1.06f, _minPlayerPositionY);
        Slider.value = t;
    }
    
    private void SetStartAndFin()
    {
        _startPlayerPositionY = Player.transform.position.y;
        _finishPositionY = FinishPlatform.position.y;
        _minPlayerPositionY = _startPlayerPositionY;
    }

}
