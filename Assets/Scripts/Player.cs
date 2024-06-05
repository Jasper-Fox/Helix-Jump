using UnityEngine;

public class Player : MonoBehaviour
{
    public float BounceStrength;
    public Rigidbody rb;
    public Game Game;
    
    internal Platform CurrentPlatform;

    public void Bounce()
    {
        rb.velocity = new Vector3(0, BounceStrength, 0); //сила вверх
    }

    public void Die()
    {
        Game.playerDied(); //сообщает игре что игрок умер
        rb.velocity = Vector3.zero; //откдючает силу на всякий случай
    }
    
    public void Win()
    {
        Game.playerWin();
        rb.velocity = Vector3.zero;
    }
}
