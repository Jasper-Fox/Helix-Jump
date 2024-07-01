using UnityEngine;
using Enums;

public class CameraFolow : MonoBehaviour
{
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
        //чтобы небыло ошибок исключаем отсутствие значения в поле CurrentPlatform 
        if (Player._currentPlatform == null) return;
        Vector3 targetPosition = StartCameraPosition + Player._currentPlatform.transform.position;
        float cameraSpeed = new float();
        cameraSpeed = CameraAcceleration(cameraSpeed);

        //MoveTowards: из A в B со скоростью C
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }

    //ускорение камеры при пропуске игроком платформ
    private float CameraAcceleration(float cameraSpeed)
    {
        //сама скорость
        cameraSpeed = CameraSpeed;

        if (Player._numberOfSkippedPlatforms > 1 && Player._speed > Player.MaxSpeed &&
            Game.CurrentState == GameState.Playing)

            //ускорение камеры 
            cameraSpeed *= Mathf.Abs(Player._speed) * CameraAccelerationVolue;

        return cameraSpeed;
    }
}