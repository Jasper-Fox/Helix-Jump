using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenetator : MonoBehaviour
{
    public GameObject Platform;
    public GameObject FinishPlatformPrefab;
    public int MinPlatformCount;
    public int MaxPlatformCount;
    public float DistanceBetweenPlatforms;

    internal int levelLenght;

    private void Awake()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        levelLenght = Random.Range(MinPlatformCount, MaxPlatformCount + 1);

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