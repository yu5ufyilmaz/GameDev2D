using System;
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

    public Image powerBar;
    private float powerCount = 0.01f;
    private bool isDone = false;
    private Coroutine powerLoop;

    private void Start()
    {
        powerLoop = StartCoroutine(PowerBar());
    }

    IEnumerator PowerBar()
    {
        powerBar.fillAmount = 0;
        isDone = false;
        while (true)
        {
            if (powerBar.fillAmount < 1 && !isDone)
            {
                powerBar.fillAmount += powerCount;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);
            }
            else
            {
                isDone = true;
                powerBar.fillAmount -= powerCount;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if (powerBar.fillAmount == 0)
                {
                    isDone = false;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ballThrowEffect, ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
            ballThrowSound.Play();
            GameObject ballObject =
                Instantiate(ball, ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
            Rigidbody2D rg = ballObject.GetComponent<Rigidbody2D>();
            rg.AddForce(new Vector2(2f, 0f) * (powerBar.fillAmount * 12f), ForceMode2D.Impulse);
            StopCoroutine(powerLoop);
        }
    }

    public void PlayPower()
    {
        powerLoop = StartCoroutine(PowerBar());
    }
}
