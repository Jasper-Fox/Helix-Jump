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
        float cameraSpeed = new float();
        cameraSpeed = CameraAcceleration(cameraSpeed);
        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition,
                cameraSpeed * Time.deltaTime); //MoveTowards: из A в B со скоростью C
    }

    private float CameraAcceleration(float cameraSpeed) //ускорение камеры при пропуске игроком платформ
    {
        cameraSpeed = CameraSpeed; //сама скорость

        if (Player._numberOfSkippedPlatforms > 1 && Player._speed > Player.MaxSpeed &&
            Game.CurrentState == GameState.Playing)
        {
            cameraSpeed *= Mathf.Abs(Player._speed) * CameraAccelerationVolue; //ускорение камеры 
        }

        return cameraSpeed;
    }
}