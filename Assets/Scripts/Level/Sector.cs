using Enums;
using UnityEngine;

public class Sector : MonoBehaviour
{
    //значение отвечающее за велечину наклона платформы от которой будет отскакивать игрок
    const float MaximumNormalVectorSlope = 0.5f;

    public SectorType CurrentType;
    public Mesh GoodSectorMesh;
    public Mesh BadSectorMesh;
    public Material GoodSectorColor;
    public Material BadSectorColor;

    internal bool _wasCollision;
    internal Platform _currentPlatform;
    internal int _numberOfSkippedPlatforms;

    private void OnCollisionEnter(Collision collision)
    {
        // проверка на наличее у столкнувшегося объекта компонента Плеер,
        // при его наличии: ТРУ и в рлеер ссылка
        if (!collision.collider.TryGetComponent(out Player player)) return;
        if (!VerticalPlane(collision)) return;
        if (CurrentType == SectorType.Null) return;

        _currentPlatform._wasCollision = true;

        if (_numberOfSkippedPlatforms > 2 || CurrentType == SectorType.Good)
        {
            if (_numberOfSkippedPlatforms > 2)
                _currentPlatform.DestroyPlatform(true);
            player.Bounce();

            //говорим игроку чтобы больше не тормозил
            player.rb.drag = 0;
        }
        else
            player.Die();
    }

    private bool VerticalPlane(Collision collision)
    {
        //вектор из точки контакта, паралельнй нормали плоскости контакта длинной 1
        Vector3 normal = -collision.contacts[0].normal.normalized;

        //сколярное произведение: 
        //если вектор нормали совпадает с вектором вверх то 1,
        //чем больше несовпадение тем меньше значение (тк cos)
        float dot = Vector3.Dot(normal, Vector3.up);

        //разрешаем прыжок только при отклонении плоскости на 60'
        if (dot >= MaximumNormalVectorSlope)
            return true;
        return false;
    }

    //для работы функции до нажатия плей
    private void OnValidate()
    {
        ChooseSectorMesh();
    }

    internal void ChooseSectorMesh()
    {
        //меш объекта (в онвалидейте вызывает кучу каких-то непонятных ошибок)
        MeshFilter sectorMash = GetComponent<MeshFilter>();

        //материал объекта
        Renderer sectorRenderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();

        //проверка все ли поля заполнены
        if (GoodSectorMesh == null || BadSectorMesh == null) return;

        switch (CurrentType)
        {
            case SectorType.Bad:
                //меняем меш
                sectorMash.mesh = BadSectorMesh;

                //меняем материал
                sectorRenderer.sharedMaterial = BadSectorColor;
                collider.isTrigger = false;
                break;

            case SectorType.Good:
                sectorMash.mesh = GoodSectorMesh;
                sectorRenderer.sharedMaterial = GoodSectorColor;
                collider.isTrigger = false;
                break;

            case SectorType.Null:
                sectorMash.mesh = null;
                sectorRenderer.sharedMaterial = null;
                collider.isTrigger = true;
                break;
        }
    }
}