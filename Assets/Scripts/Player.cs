using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject ball;
    public GameObject ballSpawnPoint;
    public ParticleSystem ballThrowEffect;
    public AudioSource ballThrowSound;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ballThrowEffect, ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
            ballThrowSound.Play();
            GameObject ballObject = Instantiate(ball, ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
            Rigidbody2D rg = ballObject.GetComponent<Rigidbody2D>();
            rg.AddForce(new Vector2(2f, 0f) * 10f, ForceMode2D.Impulse);
            
        }
    }
}
