using System;
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
        if (other.TryGetComponent(out Player player)) //если в колайдер вошел игрок то ТРУ и ссылку на компонент в плеер
        {
            player._currentPlatform = this; //записываем эту платформу в текущую игрка 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;

        CollisionLocationSearch();

        if (_wasCollision) //если было столкновение обнуляет счетчик
        {
            player._numberOfSkippedPlatforms = 0;
        }
        else
            player._numberOfSkippedPlatforms++; //иначе считает пролеты
    }

    private void CollisionLocationSearch() //проверяет есть ли столькновение хоть с одним из секторов на этой платформе
    {
        for (int i = 0; i < ThisPlatformSector.Length; i++)
        {
            if (ThisPlatformSector[i]._wasCollision) //если есть, записывает и выходит
            {
                _wasCollision = true;
                break;
            }

            _wasCollision = false;
        }
    }

    private void BuildPlatform(GameObject platform, PlatformState currentState)
    {
        float platformRotation = Random.Range(0, 360);
        for (int i = 0; i < _platform.Length; i++)
        {
            _platform[i] = Sector;

            _platform[i].name = $"Sector ({i})";

            Sector sector = Sector.GetComponent<Sector>();
            ThisPlatformSector[i] = sector;

            Quaternion rotation = new Quaternion();

            if (currentState == PlatformState.Bace)
            {
                int stateIndex = Random.Range(0, 3);

                if (stateIndex == 0)
                    sector.CurrentState = SectorState.Good;
                else if (stateIndex == 1)
                    sector.CurrentState = SectorState.Bad;
                else if (stateIndex == 2)
                    sector.CurrentState = SectorState.Null;

                rotation = Quaternion.Euler(-90, platformRotation + 45 * i, 0);
            }
            else
            {
                sector.CurrentState = SectorState.Good;

                rotation = Quaternion.Euler(-90, -45 + 45 * i, 0);
            }

            Instantiate(_platform[i], platform.transform.position, rotation, platform.transform);
        }
    }

    public void LevelGenetatorBuildPlatform(GameObject platform, PlatformState currentState)
    {
        BuildPlatform(platform, currentState);
    }
}