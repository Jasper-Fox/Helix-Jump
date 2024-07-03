using UnityEngine;
using Enums;
using Random = System.Random;
using static MyRandom.MyRandomRange;

public class PlatformGenerator : MonoBehaviour
{
    private const int SectorsNumber = 7;
    private const int MaxStateSectorIndex = 1000;

    [SerializeField] private GameObject _sector;

    [SerializeField] internal Sector[] _thisPlatformSectors = new Sector[SectorsNumber];

    internal int _levelIndex;

    private bool _wasCollision;
    private Random _random;
    private float ratioOfGoodToBadPlatforms;
    private float holeRatio;

    /// <summary>
    /// Строит платформу в данном месте данного типа из секторов
    /// </summary>
    /// <param name="random"></param>
    /// <param name="levelIndex"></param>
    /// <param name="platform"></param>
    /// <param name="currentType"></param>
    private void PlatformGeneration(Random random, int levelIndex, GameObject platform, PlatformType currentType)
    {
        int holeСounter = 0;
        _random = random;
        _levelIndex = levelIndex;

        //заполняем все сектора(на 2 цикла больше чтобы повторно проверить сектора 0 и 1 на наличее лишних отверстий)
        for (int i = 0; i < _thisPlatformSectors.Length + 2; i++)
            GenerateSector(platform, currentType, i, ref holeСounter);
    }

    /// <summary>
    /// Создаёт и настраивает новый сектор
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="currentType"></param>
    /// <param name="i"></param>
    /// <param name="holeСounter"></param>
    private void GenerateSector(GameObject platform, PlatformType currentType, int i, ref int holeСounter)
    {
        //контролируем чтоб и не вышло за массив
        int thisPlatformSectorsIndex = i % _thisPlatformSectors.Length;

        //устанавливает только по длинне массива а дальше просто проверяет, новые не создаёт
        if (i < _thisPlatformSectors.Length)
        {
            var rotation = SectorRotation(i);

            //Устанавливаем новый сектор как сектор, на место платформы, с заданным поворотом и родителем платформой
            GameObject newSector = Instantiate(_sector, platform.transform.position, rotation, platform.transform);

            //переиминовываем его
            newSector.name = $"Sector ({i})";

            //достаём из него компонент сектора
            Sector sector = newSector.GetComponent<Sector>();

            //засовываем его в массим для проверки колизии
            _thisPlatformSectors[i] = sector;
        }

        SatSectorType(currentType, _thisPlatformSectors[thisPlatformSectorsIndex], i, ref holeСounter);
    }

    public void A(ref float a)
    {
    }

    /// <summary>
    /// Сдвигает каждый сектор на 45 градусов относительно предыдущего
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private static Quaternion SectorRotation(int i)
    {
        Quaternion rotation;
        rotation = Quaternion.Euler(-90, 45 * i, 0);
        return rotation;
    }

    /// <summary>
    /// Устонавливает тип сектора
    /// </summary>
    /// <param name="currentType"></param>
    /// <param name="sector"></param>
    /// <param name="i"></param>
    /// <param name="holeСounter"></param>
    private void SatSectorType(PlatformType currentType, Sector sector, int i, ref int holeСounter)
    {
        //Обычный -> Выбираем смлучайно один из трех типов секторов
        switch (currentType)
        {
            case PlatformType.Bace:
                if (i < _thisPlatformSectors.Length)
                    ChooseOneType(sector, MaxStateSectorIndex, ref holeСounter);

                LimitNumberOfHoles(sector, i, holeСounter);
                break;
            case PlatformType.Null:
                sector.CurrentType = SectorType.Null;
                break;
            default:
                sector.CurrentType = SectorType.Good;
                break;
        }

        sector.ChooseSectorMesh();
    }

    /// <summary>
    /// Выбираем один из трех типов(случайно), и если выпадает дырка то увеличиваем счетчик дырок
    /// </summary>
    /// <param name="sector"></param>
    /// <param name="maxStateIndex"></param>
    /// <param name="holeСounter"></param>
    private void ChooseOneType(Sector sector, int maxStateIndex, ref int holeСounter)
    {
        float ratioOfGoodToBadPlatforms = 80000f / (_levelIndex + 205f) + 100;
        float holeRatio = 900 - 80000f / (_levelIndex + 705f);

        int stateIndex = RandomRange(_random, 0, maxStateIndex);

        switch (stateIndex < ratioOfGoodToBadPlatforms ? 0 : stateIndex > holeRatio ? 2 : 1)
        {
            case 0:
                sector.CurrentType = SectorType.Good;
                break;
            case 1:
                sector.CurrentType = SectorType.Bad;
                break;
            case 2:
                sector.CurrentType = SectorType.Null;
                holeСounter++;
                break;
        }
    }

    /// <summary>
    /// Тот же метод, но не выдающий пустые сектора
    /// </summary>
    /// <param name="sector"></param>
    /// <param name="holeСounter"></param>
    private void ChooseOneType(Sector sector, ref int holeСounter)
    {
        float holeRatio = 900 - 80000f / (_levelIndex + 505f);
        ChooseOneType(sector, (int)holeRatio, ref holeСounter);
    }

    /// <summary>
    /// Чтобы не создавалось лысое дерево с пробелами из 3ёх и более секторов или с тремя и менее секторами вобщем, убирает лишние дыры
    /// </summary>
    /// <param name="sector"></param>
    /// <param name="i"></param>
    /// <param name="holeCounter"></param>
    private void LimitNumberOfHoles(Sector sector, int i, int holeCounter)
    {
        //С первыми двумя работаем только после полного круга, тк вначале никакой информации о предыдущих секторах нет, с недырами не рабораем вцелом
        if (sector.CurrentType != SectorType.Null || i < 2) return;

        //Меняем отверстия если их уже более 4ёх на платформе 
        if (holeCounter > 4)
            ChooseOneType(sector, ref holeCounter);

        int pre = (i - 1) % _thisPlatformSectors.Length;
        int prepre = (i - 2) % _thisPlatformSectors.Length;

        //Меняем отверстия если 2 предыдущих тоже отверсия 
        if (_thisPlatformSectors[pre].CurrentType != SectorType.Null ||
            _thisPlatformSectors[prepre].CurrentType != SectorType.Null) return;
        ChooseOneType(sector, ref holeCounter);
    }

    /// <summary>
    /// Костыль чтобы избавиться от бага, что цикл for в BuildPlatform, при вызове из LevelGenerator работает не больше 6ти раз
    /// </summary>
    /// <param name="random"></param>
    /// <param name="levelIndex"></param>
    /// <param name="platform"></param>
    /// <param name="currentType"></param>
    public void BuildPlatform(Random random, int levelIndex, GameObject platform, PlatformType currentType)
    {
        PlatformGeneration(random, levelIndex, platform, currentType);
    }

    public void ConvertToNullPlatform()
    {
        for (int i = 0; i < _thisPlatformSectors.Length; i++)
        {
            _thisPlatformSectors[i].CurrentType = SectorType.Null;
            _thisPlatformSectors[i].ChooseSectorMesh();
        }
    }

    public void ConvertToGoodPlatform()
    {
        for (int i = 0; i < _thisPlatformSectors.Length; i++)
        {
            if (_thisPlatformSectors[i].CurrentType != SectorType.Bad) continue;

            _thisPlatformSectors[i].CurrentType = SectorType.Good;
            _thisPlatformSectors[i].ChooseSectorMesh();
        }
    }
}