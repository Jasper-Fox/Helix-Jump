using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootLenght : MonoBehaviour
{
    public LevelGenetator LevelGenetator;

    void Start()
    {
        Vector3 cylinderTransform = transform.localScale;
        cylinderTransform.y = LevelGenetator.levelLenght;
        transform.localScale = cylinderTransform;
    }
}
