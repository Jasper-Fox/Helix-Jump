using UnityEngine;
using Enums;
using Random = System.Random;

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

    //строим платформу в данном месте данного типа из секторов
    private void PlatformGenerator(Random random, GameObject platform, PlatformType currentType)
    {
        int holeСounter = 0;

        //заполняем все сектора(на 2 цикла больше чтобы повторно проверить сектора 0 и 1 на наличее лишних отверстий)
        for (int i = 0; i < ThisPlatformSectors.Length + 2; i++)
            GenerateSector(random, platform, currentType, i, ref holeСounter);
    }

    //создаём и настраиваем новый сектор
    private void GenerateSector(Random random, GameObject platform, PlatformType currentType, int i, ref int holeСounter)
    {
        //контролируем чтоб и не вышло за массив
        int thisPlatformSectorsIndex = i % ThisPlatformSectors.Length;

        //устанавливает только по длинне массива а дальше просто проверяет, новые не создаёт
        if (i < ThisPlatformSectors.Length)
        {
            var rotation = SetFirstSectorPosition(currentType, i);

            //Устанавливаем новый сектор как сектор, на место платформы, с заданным поворотом и родителем платформой
            GameObject NewSector = Instantiate(Sector, platform.transform.position, rotation, platform.transform);

            //переиминовываем его
            NewSector.name = $"Sector ({i})";

            //достаём из него компонент сектора
            Sector sector = NewSector.GetComponent<Sector>();

            //засовываем его в массим для проверки колизии
            ThisPlatformSectors[i] = sector;
        }

        SatSectorType(random, platform, currentType, ThisPlatformSectors[thisPlatformSectorsIndex], i, ref holeСounter);
    }

    //Сдвигаем каждый сектор на 45 градусов относительно предыдущего
    private static Quaternion SetFirstSectorPosition(PlatformType currentType, int i)
    {
        Quaternion rotation;
        rotation = Quaternion.Euler(-90, 45 * i, 0);
        return rotation;
    }

    //Устонавливаем тип сектора
    private void SatSectorType(Random random, GameObject platform, PlatformType currentType, Sector sector, int i, ref int holeСounter)
    {
        //Обычный -> Выбираем смлучайно один из трех типов секторов
        if (currentType == PlatformType.Bace && i < ThisPlatformSectors.Length)
        {
            int maxStateIndex = 3;

            ChooseOneType(random, sector, maxStateIndex, ref holeСounter);
            //Debug.Log($"{sector.name} = {sector.currentType}");
        }
        //если старт то все сектора хорошие
        else
            sector.currentType = SectorType.Good;

        LimitNumberOfHoles(random, platform, sector, i, holeСounter);

        sector.ChooseSectorMesh();
    }

    //Выбираем один из трех типов(случайно), и если выпадает дырка то увеличиваем счетчик дырок
    private void ChooseOneType(Random random, Sector sector, int maxStateIndex, ref int holeСounter)
    {
        int stateIndex = RandomRange(random, 0, maxStateIndex);

        switch (stateIndex)
        {
            case 0:
                sector.currentType = SectorType.Good;
                break;
            case 1:
                sector.currentType = SectorType.Bad;
                break;
            case 2:
                sector.currentType = SectorType.Null;
                holeСounter++;
                break;
        }
    }

    //Чтобы не создавалось лысое дерево с пробелами из 3ёх и более секторов или с тремя и менее секторами вобщем, убираем лишние дыры
    private void LimitNumberOfHoles(Random random, GameObject platform, Sector sector, int i, int holeСounter)
    {
        //С первыми двумя работаем только после полного круга, тк вначале никакой информации о предыдущих секторах нет, с недырами не рабораем вцелом
        if (sector.currentType != SectorType.Null || i < 2) return;

        int maxStateIndex = 2;

        //Меняем отверстия если их уже 5 и более на платформе 
        if (holeСounter > 4)
            ChooseOneType(random, sector, maxStateIndex, ref holeСounter);

        int pre = (i - 1) % ThisPlatformSectors.Length;
        int prepre = (i - 2) % ThisPlatformSectors.Length;

        //Меняем отверстия если 2 предыдущих тоже отверсия 
        if (ThisPlatformSectors[pre].currentType != SectorType.Null ||
            ThisPlatformSectors[prepre].currentType != SectorType.Null) return;
        ChooseOneType(random, sector, maxStateIndex, ref holeСounter);
        //Debug.Log($"В платфоре {platform.name} меняем сектор {sector.name} тк {ThisPlatformSectors[pre].name} = {ThisPlatformSectors[pre].currentType} и {ThisPlatformSectors[prepre].name} = {ThisPlatformSectors[prepre].currentType}");
    }

    //Костыль чтобы избавиться от бага, что цикл for в BuildPlatform, при вызове из LevelGenetator работает не больше 6ти раз
    public void BuildPlatform(Random random, GameObject platform, PlatformType currentType)
    {
        PlatformGenerator(random, platform, currentType);
    }
    
    public static int RandomRange(Random random, int min, int maxExclusive)
    {
        int number = random.Next();
        int radius = maxExclusive - min;
        
        number %= radius;
        
        return min + number;
    }

    public static float RandomRange(Random random, float min, float max)
    {
        float i = (float)random.NextDouble();

        return Mathf.Lerp(min, max, i);
    }
}