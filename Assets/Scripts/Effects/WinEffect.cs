using UnityEngine;

public class WinEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;

    public void Play()
    {
        for (int i = 0; i < _particleSystems.Length; i++)
            _particleSystems[i].Play();
    }
}