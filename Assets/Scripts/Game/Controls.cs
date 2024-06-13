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
        //если нажата лкм(0) 
        if (Input.GetMouseButton(0))
        {
            //до места нажатия
            Vector3 delta = _previousMousePosition - Input.mousePosition;
            
            //вращение по Y с указанной скоростью 
            Level.Rotate(0, delta.x * RotationSpeed, 0);
        }

        _previousMousePosition = Input.mousePosition;
    }
}