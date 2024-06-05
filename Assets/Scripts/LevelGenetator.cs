using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenetator : MonoBehaviour
    {
        public GameObject[] PlatformPrefabs;
        public int MinPlatformCount;
        public int MaxPlatformCount;
        public float DistanceBetweenPlatforms;
        
        private void Awake()
        {
            BuildLevel();
        }

        private void BuildLevel()
        {
            int levelLenght = Random.Range(MinPlatformCount, MaxPlatformCount + 1);
            
            for (int i = 0; i < levelLenght; i++)
            {
                int platformIndex = Random.Range(0, PlatformPrefabs.Length);
                Vector3 position = new Vector3(0, -DistanceBetweenPlatforms * i, 0);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                
                Instantiate(PlatformPrefabs[platformIndex], position, rotation, transform);
            }
        }
    }
