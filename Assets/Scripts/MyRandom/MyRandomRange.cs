using Random = System.Random;
using UnityEngine;

namespace MyRandom
{
    public static class MyRandomRange
    {
        public static int RandomRange(Random random, int min, int maxExclusive)
        {
            int number = random.Next();
            int radius = maxExclusive - min;

            if (radius <= 0)
                radius = 1;

            number %= radius;

            return min + number;
        }

        public static float RandomRange(Random random, float min, float max)
        {
            float i = (float)random.NextDouble();

            return Mathf.Lerp(min, max, i);
        }
    }
}