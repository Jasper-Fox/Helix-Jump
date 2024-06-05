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
        if(Player.CurrentPlatform == null) return; //чтобы небыло ушибок исключаем отсутствие значения в поле CurrentPlatform 
        Vector3 targetPosition = StartCameraPosition + Player.CurrentPlatform.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, CameraSpeed * Time.deltaTime); //MoveTowards: из A в B со скоростью C
    }
}
