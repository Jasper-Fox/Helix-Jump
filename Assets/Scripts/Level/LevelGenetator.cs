using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenetator : MonoBehaviour
{
    public GameObject Platform;
    public GameObject FinishPlatformPrefab;
    [Range(1f, 100.0f)] public int Difficulty;
    public int MinPlatformCount = 1;
    public int MaxPlatformCount = 70;
    public float DistanceBetweenPlatforms;

    internal int levelLenght;

    private float _randomOffset = 0.6f;
    private float _randomRadius = 40;

    private void Awake()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        levelLenght = Random.Range(CalculateRandomLimit(Difficulty),
            CalculateRandomLimit(Difficulty * CalculateRandomRadius()));

        //Строи уровень
        for (int i = 0; i < levelLenght; i++)
        {
            //Куда ставим платформу
            Vector3 platformPosition = new Vector3(0, -DistanceBetweenPlatforms * i, 0);

            //В го записываем установленную платформу на место, с поворотом и родителем
            GameObject go = Instantiate(Platform, platformPosition, new Quaternion(), transform);

            ChoosePlatformType(i, go);
        }

        BuildFinish();
    }

    //По фатку насколько венрхная граница отклоняется от нижней
    public float CalculateRandomRadius()
    {
        float randomRadius;

        //Сама функция
        randomRadius = MaxPlatformCount / (MaxPlatformCount + _randomRadius) + 1;
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
            offset = MinPlatformCount * MinPlatformCount / (difficulty / _randomOffset + MinPlatformCount);

        //Исключаем ошибку что минимум больше максимума
        if (MinPlatformCount > MaxPlatformCount)
            MinPlatformCount = MaxPlatformCount;
        
            //Её величество функция
            result = offset - MaxPlatformCount * (1 / (1 + difficulty / (MaxPlatformCount * _randomOffset)) - 1);
        return (int)result;
    }

    private static void ChoosePlatformType(int i, GameObject go)
    {
        //Достаём из го компанент платформа
        Platform platform = go.GetComponent<Platform>();

        //если это первая платформа то строим старт
        if (i == 0)
        {
            go.name = "Start";

            platform.BuildPlatform(go, PlatformType.Start);

            go.transform.localRotation = new Quaternion();
        }
        else
        {
            go.name = $"Platform ({i})";

            platform.BuildPlatform(go, PlatformType.Bace);

            //Случайный поворот платформы по Y
            Quaternion platformRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            go.transform.localRotation = platformRotation;
        }
    }

    private void BuildFinish()
    {
        FinishPlatformPrefab.transform.position = new Vector3(0, -DistanceBetweenPlatforms * levelLenght, 0);
        Instantiate(FinishPlatformPrefab);
    }
}