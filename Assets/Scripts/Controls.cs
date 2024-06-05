using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Transform Level;
    public float RotationSpeed;

    private Vector3 _previousMousePosition;

    void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _previousMousePosition;
            Level.Rotate(0, -delta.x * RotationSpeed, 0);
        }

        _previousMousePosition = Input.mousePosition;
    }
}
