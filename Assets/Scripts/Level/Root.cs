using UnityEngine;

public class Root : MonoBehaviour
{
    private const float t1 = 2 / 3f;
    private const float t2 = 0.8f;

    public LevelGenetator LevelGenetator;

    void Start()
    {
        LevelLengthToRootLength();
    }

    private void LevelLengthToRootLength()
    {
        Vector3 cylinderScale = transform.localScale;
        cylinderScale.y = LevelGenetator.levelLenght * t1 + t2;
        transform.localScale = cylinderScale;
    }
}