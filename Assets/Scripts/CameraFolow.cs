using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    public Game Game;
    public Player Player;
    public Vector3 StartCameraPosition;
    public float CameraSpeed;
    public float CameraAccelerationVolue;
    public float t;

    void Update()
    {
        CameraPosition();
    }

    private void CameraPosition()
    {
        if (Player._currentPlatform == null)
            return; //чтобы небыло ошибок исключаем отсутствие значения в поле CurrentPlatform 
        Vector3 targetPosition = StartCameraPosition + Player._currentPlatform.transform.position;
        var cameraSpeed = CameraAcceleration();

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraSpeed); //MoveTowards: из A в B со скоростью C
    }

    private float CameraAcceleration()
    {
        float cameraSpeed = CameraSpeed * Time.deltaTime;

        if (Player._numberOfSkippedPlatforms > 0 && Game.CurrentState == Game.State.Playing && Player._speed > Player.MaxSpeed)
        {
            cameraSpeed *= Mathf.Sqrt(t * Player._speed) * CameraAccelerationVolue;
        }

        return cameraSpeed;
    }
}