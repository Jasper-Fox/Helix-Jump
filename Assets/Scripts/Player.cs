using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public float BounceStrength;
    public Rigidbody rb;
    public Game Game;
    
    internal Platform CurrentPlatform;

    public void Bounce()
    {
        rb.velocity = new Vector3(0, BounceStrength, 0);
    }

    public void Die()
    {
        Game.playerDied();
        rb.velocity = Vector3.zero;
    }
    
    public void Win()
    {
        Game.playerWin();
        rb.velocity = Vector3.zero;
    }
}
