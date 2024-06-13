using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenetator : MonoBehaviour
{
    public GameObject Platform;
    public GameObject StartPlatformPrefab;
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

        for (int i = 0; i < levelLenght; i++)
        {
            Vector3 platformPosition = new Vector3(0, -DistanceBetweenPlatforms * i, 0);
            GameObject go = Instantiate(Platform, platformPosition, new Quaternion(), transform);
            Platform platform = go.GetComponent<Platform>();
            if (i == 0)
            {
                go.name = "Start";
                platform.LevelGenetatorBuildPlatform(go, PlatformState.Start);
            }
            else
            {
                go.name = $"Platform {i}";
                platform.LevelGenetatorBuildPlatform(go, PlatformState.Bace);
            }
        }

        BuildFinish();
    }

    private void BuildFinish()
    {
        FinishPlatformPrefab.transform.position = new Vector3(0, -DistanceBetweenPlatforms * levelLenght, 0);
        Instantiate(FinishPlatformPrefab);
    }
}