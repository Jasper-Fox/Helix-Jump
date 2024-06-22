using Enums;
using UnityEngine;
using Random = System.Random;
using static MyRandom.MyRandomRange;

public class LevelGenetator : MonoBehaviour
{
    private const float _randomOffsetForLevelLenght = 0.6f;
    private const float _randomRadiusForLevelLenght = 40;

    public Game Game;
    public GameObject Platform;
    public GameObject FinishPlatformPrefab;

    public int MinPlatformCount = 1;
    public int MaxPlatformCount = 70;
    public float DistanceBetweenPlatforms;
    [Range(1, 1000)] public int LevelGenerationKey;

    internal int levelLenght;

    private int _difficulty = 5;

    private void Awake()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        CalculateDifficulty();

        var random = LevelRandomKey();

        levelLenght = RandomRange(random, CalculateRandomLimit(_difficulty),
            CalculateRandomLimit(_difficulty * CalculateRandomRadius()));

        //Строи уровень
        for (int i = 0; i < levelLenght; i++)
        {
            //Куда ставим платформу
            Vector3 platformPosition = new Vector3(0, -DistanceBetweenPlatforms * i, 0);

            //В го записываем установленную платформу на место, с поворотом и родителем
            GameObject go = Instantiate(Platform, platformPosition, new Quaternion(), transform);

            ChoosePlatformType(random, i, go);
        }

        BuildFinish();
    }

    //создаём рандом со случайным ключем генерации
    private Random LevelRandomKey()
    {
        Random levelKeyGenerator = new Random(LevelGenerationKey);

        int levelGenerationKey = levelKeyGenerator.Next();

        Random random = new Random(Game.LevelIndex + levelGenerationKey);
        return random;
    }

    private void CalculateDifficulty()
    {
        _difficulty = Game.LevelIndex + 5;
    }

    //По фатку насколько венрхная граница отклоняется от нижней
    public float CalculateRandomRadius()
    {
        float randomRadius;

        //Сама функция
        randomRadius = MaxPlatformCount / (MaxPlatformCount + _randomRadiusForLevelLenght) + 1;
        return randomRadius;
    }

    //Функция по которой будут определяться границы
    private int CalculateRandomLimit(float difficulty)
    {
        float result;
        float offset;

        //Доопределяем ноль на случай если и сложность и минимум будут 0
        if (MinPlatformCount < 1)
            offset = 0;
        else
            offset = MinPlatformCount * MinPlatformCount /
                     (difficulty / _randomOffsetForLevelLenght + MinPlatformCount);

        //Исключаем ошибку что минимум больше максимума
        if (MinPlatformCount > MaxPlatformCount)
            MinPlatformCount = MaxPlatformCount;

        //Её величество функция
        result = offset - MaxPlatformCount *
            (1 / (1 + difficulty / (MaxPlatformCount * _randomOffsetForLevelLenght)) - 1);
        return (int)result;
    }

    private void ChoosePlatformType(Random random, int i, GameObject go)
    {
        //Достаём из го компанент платформа
        Platform platform = go.GetComponent<Platform>();

        //если это первая платформа то строим старт
        if (i == 0)
        {
            go.name = "Start";

            platform.BuildPlatform(random, Game.LevelIndex, go, PlatformType.Start);

            go.transform.localRotation = new Quaternion();
        }
        else
        {
            go.name = $"Platform ({i})";

            platform.BuildPlatform(random, Game.LevelIndex, go, PlatformType.Bace);

            //Случайный поворот платформы по Y
            Quaternion platformRotation = Quaternion.Euler(0, RandomRange(random, 0, 360), 0);
            go.transform.localRotation = platformRotation;
        }
    }

    private void BuildFinish()
    {
        FinishPlatformPrefab.transform.position = new Vector3(0, -DistanceBetweenPlatforms * levelLenght, 0);
        Instantiate(FinishPlatformPrefab);
    }
}