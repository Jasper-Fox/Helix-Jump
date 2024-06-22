using UnityEngine;
using Enums;
using Random = System.Random;
using static MyRandom.MyRandomRange;

public class PlatformGenerator : MonoBehaviour
{
    private const int SectorsNumber = 7;

    [SerializeField] private GameObject Sector;
    [SerializeField] private Sector[] ThisPlatformSectors = new Sector[SectorsNumber];

    private bool _wasCollision;
    private int _levelIndex;
    private Random _random;
    private float ratioOfGoodToBadPlatforms;
    private float holeRatio;
    
    private void BuildPlatform(Random random, int levelIndex, GameObject platform, PlatformType currentType)
    {
        int holeСounter = 0;
        _random = random;
        _levelIndex = levelIndex;
        ratioOfGoodToBadPlatforms = 80000f / (_levelIndex + 205f) + 100;
        holeRatio = 900 - 80000f / (_levelIndex + 505f);

        //заполняем все сектора(на 2 цикла больше чтобы повторно проверить сектора 0 и 1 на наличее лишних отверстий)
        for (int i = 0; i < ThisPlatformSectors.Length + 2; i++)
            GenerateSector(platform, currentType, i, ref holeСounter);
    }

    //создаём и настраиваем новый сектор
    private void GenerateSector(GameObject platform, PlatformType currentType, int i, ref int holeСounter)
    {
        //контролируем чтоб и не вышло за массив
        int thisPlatformSectorsIndex = i % ThisPlatformSectors.Length;

        //устанавливает только по длинне массива а дальше просто проверяет, новые не создаёт
        if (i < ThisPlatformSectors.Length)
        {
            var rotation = SectorRotation(i);

            //Устанавливаем новый сектор как сектор, на место платформы, с заданным поворотом и родителем платформой
            GameObject NewSector = Instantiate(Sector, platform.transform.position, rotation, platform.transform);

            //переиминовываем его
            NewSector.name = $"Sector ({i})";

            //достаём из него компонент сектора
            Sector sector = NewSector.GetComponent<Sector>();

            //засовываем его в массим для проверки колизии
            ThisPlatformSectors[i] = sector;
        }

        SatSectorType(platform, currentType, ThisPlatformSectors[thisPlatformSectorsIndex], i, ref holeСounter);
    }

    //Сдвигаем каждый сектор на 45 градусов относительно предыдущего
    private static Quaternion SectorRotation(int i)
    {
        Quaternion rotation;
        rotation = Quaternion.Euler(-90, 45 * i, 0);
        return rotation;
    }

    //Устонавливаем тип сектора
    private void SatSectorType(GameObject platform, PlatformType currentType, Sector sector, int i, ref int holeСounter)
    {
        //Обычный -> Выбираем смлучайно один из трех типов секторов
        if (currentType == PlatformType.Bace)
        {
            if (i < ThisPlatformSectors.Length)
            {
                int maxStateIndex = 1000;

                ChooseOneType(sector, maxStateIndex, ref holeСounter);
                //Debug.Log($"{sector.name} = {sector.currentType}");
            }

            LimitNumberOfHoles(platform, sector, i, holeСounter);
        }
        //если старт то все сектора хорошие
        else
            sector.currentType = SectorType.Good;

        sector.ChooseSectorMesh();
    }

    //Выбираем один из трех типов(случайно), и если выпадает дырка то увеличиваем счетчик дырок
    private void ChooseOneType(Sector sector, int maxStateIndex, ref int holeСounter)
    {
        int stateIndex = RandomRange(_random, 0, maxStateIndex);

        switch (stateIndex < ratioOfGoodToBadPlatforms ? 0 : stateIndex > holeRatio ? 2 : 1)
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
    private void LimitNumberOfHoles(GameObject platform, Sector sector, int i, int holeCounter)
    {
        //С первыми двумя работаем только после полного круга, тк вначале никакой информации о предыдущих секторах нет, с недырами не рабораем вцелом
        if (sector.currentType != SectorType.Null || i < 2) return;

        int maxStateIndex = (int)holeRatio;

        //Меняем отверстия если их уже более 4ёх на платформе 
        if (holeCounter > 4)
            ChooseOneType(sector, maxStateIndex, ref holeCounter);

        int pre = (i - 1) % ThisPlatformSectors.Length;
        int prepre = (i - 2) % ThisPlatformSectors.Length;

        //Меняем отверстия если 2 предыдущих тоже отверсия 
        if (ThisPlatformSectors[pre].currentType != SectorType.Null ||
            ThisPlatformSectors[prepre].currentType != SectorType.Null) return;
        ChooseOneType(sector, maxStateIndex, ref holeCounter);
        //Debug.Log($"В платфоре {platform.name} меняем сектор {sector.name} тк {ThisPlatformSectors[pre].name} = {ThisPlatformSectors[pre].currentType} и {ThisPlatformSectors[prepre].name} = {ThisPlatformSectors[prepre].currentType}");
    }

    // //Костыль чтобы избавиться от бага, что цикл for в BuildPlatform, при вызове из LevelGenetator работает не больше 6ти раз
    // public void BuildPlatform(Random random, int levelIndex, GameObject platform, PlatformType currentType)
    // {
    //     PlatformGenerator(random, levelIndex, platform, currentType);
    // }
}