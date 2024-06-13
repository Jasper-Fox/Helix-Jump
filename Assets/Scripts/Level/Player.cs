using UnityEngine;
using Enums;

public class Player : MonoBehaviour
{
    public float MaxSpeed;
    public float BounceStrength;
    public Rigidbody rb;
    public Game Game;

    internal Platform _currentPlatform;
    internal int _numberOfSkippedPlatforms;
    internal float _speed;

    private Vector3 _lastPosition;
    private float BrakingForce;
    private int i;

    private void Awake()
    {
        _lastPosition = transform.position;

        BrakingCalculation();
    }

    private void Update()
    {
        SpeedСalculation();
    }

    private void SpeedСalculation()
    {
        _speed = _lastPosition.y - transform.position.y;

        if (_speed > MaxSpeed)
        {
            rb.drag = _speed * BrakingForce;
        }

        _lastPosition = transform.position;
    }

    private void BrakingCalculation()
    {
        BrakingForce = 1 / (2 * MaxSpeed * MaxSpeed) + 0.2f;
    }

    public void Bounce()
    {
        //фикс бага с застреванием на границе киллзоны сектора
        if (Game.CurrentState == GameState.Playing)
            //сила вверх
            rb.velocity = new Vector3(0, BounceStrength, 0); 
    }

    public void Die()
    {
        //сообщает игре что игрок умер
        Game.playerDied();
        
        //откдючает силу на всякий случай
        rb.velocity = Vector3.zero;
    }

    public void Win()
    {
        Game.playerWin();
        rb.velocity = Vector3.zero;
    }
}