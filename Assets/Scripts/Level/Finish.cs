using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.TryGetComponent(out Player player)) return; // проверка на наличее у столкнувшегося объекта компонента Плеер,
                                                                           // при его наличии: ТРУ и в рлеер ссылка
        player.Win();
    }
}
