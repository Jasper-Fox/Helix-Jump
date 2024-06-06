using UnityEngine;

public class Sector : MonoBehaviour
{
    const float
        MaximumNormalVectorSlope =
            0.5f; //значение отвечающее за велечину наклона платформы от которой будет отскакивать игрок

    public State CurrentState;

    public enum State
    {
        Good,
        Bad,
        Null,
    }
    public Mesh GoodSectorMesh;
    public Mesh BadSectorMesh;
    public Material GoodSectorColor;
    public Material BadSectorColor;

    internal bool _wasCollision;

    private void Awake()
    {
        ChooseSectorMesh();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.TryGetComponent(out Player player))
            return; // проверка на наличее у столкнувшегося объекта компонента Плеер,
        // при его наличии: ТРУ и в рлеер ссылка
        if (!VerticalPlane(collision)) return;
        if(CurrentState == State.Null)return;

        if (!_wasCollision) //запоминает что было столкновение
            _wasCollision = true;
        
        if (CurrentState == State.Bad)
            player.Die();
        else if (CurrentState == State.Good)
            player.Bounce();
    }

    private bool VerticalPlane(Collision collision)
    {
        Vector3
            normal = -collision.contacts[0].normal
                .normalized; //вектор из точки контакта, паралельнй нормали плоскости контакта длинной 1
        float dot = Vector3.Dot(normal, Vector3.up); //сколярное произведение: 
        //если вектор нормали совпадает с вектором вверх то 1,
        //чем больше несовпадение тем меньше значение (тк cos)
        if (dot >= MaximumNormalVectorSlope) //разрешаем прыжок только при отклонении плоскости на 60'
            return true;
        return false;
    }

    private void OnValidate() //для рабы функции до нажатия плей
    {
        ChooseSectorMesh();
    }

    private void ChooseSectorMesh()
    {
        MeshFilter sectorMash = GetComponent<MeshFilter>(); //меш объекта (в онвалидейте вызывает кучу каких-то непонятных ошибок)
        Renderer sectorRenderer = GetComponent<Renderer>(); //материал объекта
        Collider collider = GetComponent<Collider>();

        if (GoodSectorMesh == null || BadSectorMesh == null) return; //проверка все ли поля заполнены 

        if (CurrentState == State.Bad)
        {
            sectorMash.mesh = BadSectorMesh; //меняем меш
            sectorRenderer.sharedMaterial = BadSectorColor; //меняем материал
            collider.isTrigger = false;
        }
        else if(CurrentState == State.Good)
        {
            sectorMash.mesh = GoodSectorMesh;
            sectorRenderer.sharedMaterial = GoodSectorColor;
            collider.isTrigger = false;
        }
        else if (CurrentState == State.Null)
        {
            sectorMash.mesh = null;
            sectorRenderer.sharedMaterial = null;
            collider.isTrigger = true;
        }
    }
}