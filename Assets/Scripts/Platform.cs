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

        if (_wasCollision) //если было столкновение обнуляет счетчик
        {
            player._numberOfSkippedPlatforms = 0;
        }
        else
            player._numberOfSkippedPlatforms++; //иначе считает пролеты
    }

    private void CollisionLocationSearch() //проверяет есть ли столькновение хоть с одним из секторов на этой платформе
    {
        for (int i = 0; i < ThisPlatformSector.Length; i++)
        {
            if (ThisPlatformSector[i]._wasCollision) //если есть, записывает и выходит
            {
                _wasCollision = true;
                break;
            }

            _wasCollision = false;
        }
    }
}