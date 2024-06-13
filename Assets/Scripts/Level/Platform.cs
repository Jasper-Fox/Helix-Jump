using UnityEngine;
using Enums;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    private const int SectorsNumber = 7;

    public GameObject Sector;

    private GameObject[] _platform = new GameObject[SectorsNumber];
    private Sector[] ThisPlatformSector = new Sector[SectorsNumber];
    private bool _wasCollision;

    private void OnTriggerEnter(Collider other)
    {
        //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        if (other.TryGetComponent(out Player player))
        {
            //записываем эту платформу в текущую игрка 
            player._currentPlatform = this; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        CollisionLocationSearch();

        //если было столкновение обнуляет счетчик
        if (_wasCollision) 
        {
            player._numberOfSkippedPlatforms = 0;
        }
        else
            //иначе считает пролеты
            player._numberOfSkippedPlatforms++;
    }

    //проверяет есть ли столькновение хоть с одним из секторов на этой платформе
    private void CollisionLocationSearch() 
    {
        for (int i = 0; i < ThisPlatformSector.Length; i++)
        {
            //если есть, записывает и выходит
            if (ThisPlatformSector[i]._wasCollision) 
            {
                _wasCollision = true;
                break;
            }

            _wasCollision = false;
        }
    }

    //строим платформу в данном месте данного типа
    private void BuildPlatform(GameObject platform, PlatformType currentType)
    {
        //задаём случайный поворот начального сектора
        float platformRotation = Random.Range(0, 360); 
        
        //заполняем все сектора
        for (int i = 0; i < _platform.Length; i++)
        {
            //вставляем сектор
            _platform[i] = Sector;

            //переиминовываем его
            _platform[i].name = $"Sector ({i})";

            //достаём из него компонент сектора
            Sector sector = Sector.GetComponent<Sector>();
            
            //засовываем его в массим для проверки колизии
            ThisPlatformSector[i] = sector;

            //Устонавливаем тип сектора;
            Quaternion rotation = SatSectorType(currentType, sector, platformRotation, i);
            
            //ставим его
            Instantiate(_platform[i], platform.transform.position, rotation, platform.transform);
        }
    }

    private static Quaternion SatSectorType(PlatformType currentType, Sector sector, float platformRotation, int i)
    {
        Quaternion rotation;
        
        //Обычный -> Выбираем смлучайно один из трех типов секторов и случайный нальный поворот
        if (currentType == PlatformType.Bace) 
        {
            int stateIndex = Random.Range(0, 3);

            if (stateIndex == 0)
                sector.currentType = SectorType.Good;
            else if (stateIndex == 1)
                sector.currentType = SectorType.Bad;
            else if (stateIndex == 2)
                sector.currentType = SectorType.Null;

            rotation = Quaternion.Euler(-90, platformRotation + 45 * i, 0);
        }
        //если старт то все сектора хорошие и поворот постоянный
        else
        {
            sector.currentType = SectorType.Good;

            rotation = Quaternion.Euler(-90, -45 + 45 * i, 0);
        }

        return rotation;
    }

    //Костыль чтобы избавиться от бага, что цикл for в BuildPlatform, при вызове из LevelGenetator работает не больше 6ти раз
    public void
        LevelGenetatorBuildPlatform(GameObject platform, PlatformType currentType)
    {
        BuildPlatform(platform, currentType);
    }
}