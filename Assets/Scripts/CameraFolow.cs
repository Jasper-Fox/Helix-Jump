using UnityEngine;
using Enums;

public class CameraFolow : MonoBehaviour
{
    public const float t = 40;

    public Game Game;
    public Player Player;
    public Vector3 StartCameraPosition;
    public float CameraSpeed;
    public float CameraAccelerationVolue;
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

    private float CameraAcceleration() //ускорение камеры при пропуске игроком платформ
    {
        float cameraSpeed = CameraSpeed * Time.deltaTime; //сама скорость

        if (Player._numberOfSkippedPlatforms > 0 && Game.CurrentState == GameState.Playing && Player._speed > Player.MaxSpeed)
        {
            cameraSpeed *= Mathf.Sqrt(Mathf.Sqrt(t * Player._speed)) * CameraAccelerationVolue; //ускорение камеры по корню 
        }

        return cameraSpeed;
    }
}