using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    public Player Player;
    public Vector3 StartCameraPosition;
    public float CameraSpeed;

    void Update()
    {
        CameraPosition();
    }

    private void CameraPosition()
    {
        if(Player._currentPlatform == null) return; //чтобы небыло ошибок исключаем отсутствие значения в поле CurrentPlatform 
        Vector3 targetPosition = StartCameraPosition + Player._currentPlatform.transform.position;
        var cameraSpeed = CameraAcceleration();

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraSpeed); //MoveTowards: из A в B со скоростью C
    }

    private float CameraAcceleration()
    {
        float cameraSpeed = CameraSpeed * Time.deltaTime;

        if (Player._numberOfSkippedPlatforms > 1)
        {
            cameraSpeed *= Mathf.Log(Player._numberOfSkippedPlatforms) * 2f;
        }

        return cameraSpeed;
    }
}
