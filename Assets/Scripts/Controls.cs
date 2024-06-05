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
        if (Input.GetMouseButton(0)) //если нажата лкм(0) 
        {
            Vector3 delta = _previousMousePosition - Input.mousePosition; //до места нажатия
            Level.Rotate(0, delta.x * RotationSpeed, 0); //вращение по Y с указанной скоростью 
        }

        _previousMousePosition = Input.mousePosition;
    }
}
