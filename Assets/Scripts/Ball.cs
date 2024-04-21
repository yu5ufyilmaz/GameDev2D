using UnityEngine;

public class Ball : MonoBehaviour
{
    float hitPower;
    GameObject gameControl;

    void Start()
    {
        hitPower = 20;
        gameControl = GameObject.FindWithTag("GameControl");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      

        if (collision.gameObject.CompareTag("BoxInMid"))
        {
            collision.gameObject.GetComponent<BoxInMid>().TakeHit(hitPower);

            gameControl.GetComponent<GameController>().CreateEffects(1,collision.gameObject);

            Destroy(gameObject);
         
          
        }
        if (collision.gameObject.CompareTag("Player2Tower"))
        {
            gameControl.GetComponent<GameController>().CreateEffects(1, collision.gameObject);
            gameControl.GetComponent<GameController>().Hit(2, hitPower);
            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Player1Tower"))
        {
            gameControl.GetComponent<GameController>().CreateEffects(1, collision.gameObject);
            gameControl.GetComponent<GameController>().Hit(1, hitPower);
            Destroy(gameObject);

        }
    }
    
}
