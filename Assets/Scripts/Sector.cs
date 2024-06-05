using UnityEngine;

public class Sector : MonoBehaviour
{
    const float MaximumNormalVectorSlope = 0.5f; //значение отвечающее за велечину наклона платформы от которой будет отскакивать игрок

    public bool Bad;
    public Mesh GoodSectorMesh;
    public Mesh BadSectorMesh;
    public Material GoodSectorColor;
    public Material BadSectorColor;

    private void Awake()
    {
        ChooseSectorMesh();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.TryGetComponent(out Player player)) return;// проверка на наличее у столкнувшегося объекта компонента Плеер,
                                                                           // при его наличии: ТРУ и в рлеер ссылка
        if (!VerticalPlane(collision)) return;
        if (Bad)
        {
            player.Die();
        }
        else
            player.Bounce();
    }

    private bool VerticalPlane(Collision collision)
    {
        Vector3 normal = -collision.contacts[0].normal.normalized; //вектор из точки контакта, паралельнй нормали плоскости контакта длинной 1
        float dot = Vector3.Dot(normal, Vector3.up); //сколярное произведение: 
                                                            //если вектор нормали совпадает с вектором вверх то 1,
                                                            //чем больше несовпадение тем меньше значение (тк cos)
        if (dot >= MaximumNormalVectorSlope)//разрешаем прыжок только при отклонении плоскости на 60'
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
        
        if(GoodSectorMesh == null || BadSectorMesh == null) return; //проверка все ли поля заполнены 
        
        if (Bad)
        {
            sectorMash.mesh = BadSectorMesh; //меняем меш
            sectorRenderer.sharedMaterial = BadSectorColor; //меняем материал
        }
        else
        {
            sectorMash.mesh = GoodSectorMesh;
            sectorRenderer.sharedMaterial = GoodSectorColor; 
        }
    }
}