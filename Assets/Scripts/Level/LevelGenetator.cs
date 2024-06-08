using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenetator : MonoBehaviour
    {
        public GameObject[] PlatformPrefabs;
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
            
            BuildStart();
            
            for (int i = 0; i < levelLenght - 1; i++)
            {
                int platformIndex = Random.Range(0, PlatformPrefabs.Length);
                Vector3 position = new Vector3(0, -DistanceBetweenPlatforms * (i+1), 0);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                
                Instantiate(PlatformPrefabs[platformIndex], position, rotation, transform);
            }
            
            BuildFinish();
        }

        private void BuildFinish()
        {
            Vector3 position = new Vector3(0, -DistanceBetweenPlatforms * levelLenght, 0);
            Instantiate(FinishPlatformPrefab, position, new Quaternion(), transform);
        }

        private void BuildStart()
        {
            Instantiate(StartPlatformPrefab, transform);
        }
    }
