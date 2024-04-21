using UnityEngine;

public class Ball : MonoBehaviour
{
    float _hitPower;
    private GameObject Player;
    void Start()
    {
        _hitPower = 20;
        Player = GameObject.FindWithTag("Player1");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxInMid"))
        {
            collision.gameObject.GetComponent<BoxInMid>().TakeHit(_hitPower);
            GameController.instance.CreateEffects(1,collision.gameObject);
            Player.GetComponent<Player>().PlayPower();
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Player1Tower") || collision.gameObject.CompareTag("Player1"))
        {
            GameController.instance.CreateEffects(1, collision.gameObject);
            GameController.instance.Hit(1, _hitPower);
            Player.GetComponent<Player>().PlayPower();
            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Player2Tower") || collision.gameObject.CompareTag("Player2"))
        {
            GameController.instance.CreateEffects(1, collision.gameObject);
            GameController.instance.Hit(2, _hitPower);
            Player.GetComponent<Player>().PlayPower();
            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameController.instance.CreateEffects(1, collision.gameObject);
            Destroy(gameObject);
        }
    }
    
}
