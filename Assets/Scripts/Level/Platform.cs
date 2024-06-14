using UnityEngine;
using Enums;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Platform : MonoBehaviour
{
    private const int SectorsNumber = 7;

    public GameObject Sector;
    
    [SerializeField] private Sector[] ThisPlatformSectors = new Sector[SectorsNumber];
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
        for (int i = 0; i < ThisPlatformSectors.Length; i++)
        {
            //если есть, записывает и выходит
            if (ThisPlatformSectors[i]._wasCollision)
            {
                _wasCollision = true;
                break;
            }

            _wasCollision = false;
        }
    }

    //строим платформу в данном месте данного типа
    private void PlatformGenerator(GameObject platform, PlatformType currentType)
    {
        //задаём случайный поворот начального сектора
        float platformRotation = Random.Range(0, 360);

        //заполняем все сектора
        for (int i = 0; i < ThisPlatformSectors.Length; i++)
        {
            var rotation = SetFirstSectorPosition(currentType, platformRotation, i);

            GameObject NewSector = Instantiate(Sector, platform.transform.position, rotation, platform.transform);
            
            //переиминовываем его
            NewSector.name = $"Sector ({i})";

            //достаём из него компонент сектора
            Sector sector = NewSector.GetComponent<Sector>();

            //засовываем его в массим для проверки колизии
            ThisPlatformSectors[i] = sector;
            
            //Устонавливаем тип сектора;
            SatSectorType(currentType, sector, i);

            sector.ChooseSectorMesh();
        }
    }

    private static Quaternion SetFirstSectorPosition(PlatformType currentType, float platformRotation, int i)
    {
        Quaternion rotation;
        if (currentType == PlatformType.Bace)
            rotation = Quaternion.Euler(-90, platformRotation + 45 * i, 0);
        else
            rotation = Quaternion.Euler(-90, -45 + 45 * i, 0);
        return rotation;
    }

    private void SatSectorType(PlatformType currentType, Sector sector, int i)
    {
        //Обычный -> Выбираем смлучайно один из трех типов секторов
        if (currentType == PlatformType.Bace)
        {
            int stateIndex = Random.Range(0, 3);

            if (stateIndex == 0)
                sector.currentType = SectorType.Good;
            else if (stateIndex == 1)
                sector.currentType = SectorType.Bad;
            else if (stateIndex == 2)
            {
                if (i > 1 && ThisPlatformSectors[i - 1].currentType == SectorType.Null &&
                    ThisPlatformSectors[i - 2].currentType == SectorType.Null)
                {
                    stateIndex = Random.Range(0, 2);
                    if (stateIndex == 0)
                        sector.currentType = SectorType.Good;
                    else if (stateIndex == 1)
                        sector.currentType = SectorType.Bad;
                }
                else
                    sector.currentType = SectorType.Null;
            }
        }
        //если старт то все сектора хорошие
        else
            sector.currentType = SectorType.Good;
    }

    //Костыль чтобы избавиться от бага, что цикл for в BuildPlatform, при вызове из LevelGenetator работает не больше 6ти раз
    public void BuildPlatform(GameObject platform, PlatformType currentType)
    {
        PlatformGenerator(platform, currentType);
    }
}