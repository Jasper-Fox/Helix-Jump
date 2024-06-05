using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player)) //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
            player.CurrentPlatform = this; //записываем эту платформу в текущую игрка 
    }
}
