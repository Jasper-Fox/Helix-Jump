using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    private const string SoundStateKey = "soundState";

    [Min(0)] [SerializeField] private float _lerpMuteSpeed = 0.001f;

    [SerializeField] internal AudioSource _audioSource;

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _onSounds;
    [SerializeField] private Sprite _offSounds;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _destructionSound;

    internal bool _lerpMute;
    internal float _actualSoundVolume;

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
        if (!_lerpMute)
            _actualSoundVolume = _audioSource.volume;

        if (_lerpMute && _audioSource.volume > 0)
            LerpMute();
    }

    private void LerpMute()
    {
        _audioSource.volume -= _lerpMuteSpeed;
    }

    private void MuteAudio()
    {
        if (!_audioSource.mute)
        {
            _audioSource.mute = true;
            _image.sprite = _offSounds;
            SoundState = 0;
        }
        else
        {
            _audioSource.mute = false;
            _image.sprite = _onSounds;
            SoundState = 1;
        }
    }

    public void Click()
    {
        _audioSource.PlayOneShot(_click);
    }

    public void Collision()
    {
        _audioSource.PlayOneShot(_collisionSound);
    }

    public void Destruction()
    {
        _audioSource.PlayOneShot(_destructionSound);
    }
}