using UnityEngine;
using UnityEngine.UI;

public class ProcessBar : MonoBehaviour
{
    private const float HalfThicknessFinishingPlatform = 1.06f;
    
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
        _playerPositionY = Player.transform.position.y; //при каждом проходе обновляет значение позиции 
        _minPlayerPositionY = Mathf.Min(_playerPositionY ,  _minPlayerPositionY); 
        float t = Mathf.InverseLerp(_startPlayerPositionY, _finishPositionY + HalfThicknessFinishingPlatform, _minPlayerPositionY);//обратная линейная функция от А = 0 до В = 1,
                                                                                                                                             //чем меньше позиция игрока тем больше значение прогресбара
        Slider.value = t;
    }
    
    private void SetStartAndFin()
    {
        _startPlayerPositionY = Player.transform.position.y;
        _finishPositionY = FinishPlatform.position.y;
        _minPlayerPositionY = _startPlayerPositionY;
    }

}
