using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Sector ThisPlatformSector;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player)) //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        {
            player._currentPlatform = this; //записываем эту платформу в текущую игрка 
            
            if(ThisPlatformSector == null) return;
               ThisPlatformSector._wasCollision = false;

               Debug.Log($"Вошел " + ThisPlatformSector._wasCollision);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (ThisPlatformSector._wasCollision)
            {
                player._numberOfSkippedPlatforms = 0;
                Debug.Log("Обнуляемся");
            }        
            else
                player._numberOfSkippedPlatforms++;
            
            Debug.Log("Вышел" + ThisPlatformSector._wasCollision);
            Debug.Log(player._numberOfSkippedPlatforms);
        }
    }
}