using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Sector[] ThisPlatformSector;

    private bool _wasCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player)) //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        {
            player._currentPlatform = this; //записываем эту платформу в текущую игрка 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        CollisionLocationSearch();

        if (_wasCollision)
        {
            player._numberOfSkippedPlatforms = 0;
        }
        else
            player._numberOfSkippedPlatforms++;
    }

    private void CollisionLocationSearch()
    {
        for (int i = 0; i < ThisPlatformSector.Length; i++)
        {
            if (ThisPlatformSector[i]._wasCollision)
            {
                _wasCollision = true;
                break;
            }

            _wasCollision = false;
        }
    }
}