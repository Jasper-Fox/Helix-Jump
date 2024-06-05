using System.Collections;
using System.Collections.Generic;
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
        if(Player.CurrentPlatform == null) return;
        Vector3 targetPosition = StartCameraPosition + Player.CurrentPlatform.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, CameraSpeed * Time.deltaTime);
    }
}
