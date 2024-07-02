using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    private const string SoundStateKey = "soundState";
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _onSounds;
    [SerializeField] private Sprite _offSounds;
    [Min(0)][SerializeField] private float _lerpMuteSpeed = 0.001f;
    
    internal bool _lerpMute;

    public int SoundState
    {
        get => PlayerPrefs.GetInt(SoundStateKey, 1);
        set 
        {
            PlayerPrefs.SetInt(SoundStateKey, value);
            PlayerPrefs.Save();
        } 
    }

    private void Awake()
    {
        if (SoundState == 0)
            MuteAudio();
        _button.onClick.AddListener(MuteAudio);
    }

    private void FixedUpdate()
    {
        if (_lerpMute && _audioSource.volume > 0)
            LerpMute();
    }

    private void LerpMute()
    {
        _audioSource.volume -= _lerpMuteSpeed;
    }

    private void MuteAudio()
    {
        if (_audioSource.volume > 0)
        {
            _audioSource.volume = 0;
            _image.sprite = _offSounds;
            SoundState = 0;
        }
        else
        {
            _audioSource.volume = 0.1f;
            _image.sprite = _onSounds;
            SoundState = 1;
        }
    }
}