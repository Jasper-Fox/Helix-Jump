using UnityEngine;
using Enums;

public class Player : MonoBehaviour
{
    private const string NumberOfPassedPlatformsKey = "numberOfPassedPlatforms";
    
    public float MaxSpeed;
    public float BounceStrength;
    public Rigidbody rb;
    public Game Game;
    [SerializeField] internal SoundControl _soundControl;
    
    internal int _numberOfSkippedPlatforms;
    internal float _speed;
    internal Platform _currentPlatform;

    private float BrakingForce;
    private Vector3 _lastPosition;

    public int NumberOfPassedPlatforms
    {
        get => PlayerPrefs.GetInt(NumberOfPassedPlatformsKey, 0);
        set
        {
            PlayerPrefs.SetInt(NumberOfPassedPlatformsKey, value);
            PlayerPrefs.Save();
        } 
    }

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
        if (Game.CurrentState == GameState.Playing || Game.CurrentState == GameState.Start)
            //сила вверх
            rb.velocity = new Vector3(0, BounceStrength, 0); 
    }

    public void Die()
    {
        //сообщает игре что игрок умер
        Game.PlayerDied();
        
        //откдючает силу на всякий случай
        rb.velocity = Vector3.zero;
    }

    public void Win()
    {
        Game.PlayerWin();
        
        rb.velocity = Vector3.zero;
    }

    public void Reborn()
    {
        Game.PlayerReborn();
        
        rb.velocity = new Vector3(0, BounceStrength, 0);
    }
}