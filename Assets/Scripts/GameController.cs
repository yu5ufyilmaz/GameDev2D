using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballDestroyEffect;
    public AudioSource destroySound;

    [Header("Box Settings")]
    public GameObject boxDestroyEffect;
    public AudioSource boxDestroySound;
    [Header("Player Health")]
    public Image player1HealthBar;
    float player1Health=100;
    public Image player2HealthBar;
    float player2Health=100;

    public static GameController instance;
    private void Awake() 
    { 
        // Bir örnek varsa ve ben değilse, yoket.
        
        if (instance != null && instance != this) 
        { 
            Destroy(this);
            return;
        } 
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CreateEffects(int criterion,GameObject objectTransform)
    {
        switch (criterion)
        {

            case 1:
                Instantiate(ballDestroyEffect, objectTransform.gameObject.transform.position, objectTransform.gameObject.transform.rotation);
                destroySound.Play();
                break;
            case 2:
                Instantiate(boxDestroyEffect, objectTransform.gameObject.transform.position, objectTransform.gameObject.transform.rotation);
                boxDestroySound.Play();
                break;
        }
        
    }
    
    public void Hit(int criterion,float hitPower)
    {
        switch (criterion)
        {

            case 1:
                player1Health -= hitPower;

                player1HealthBar.fillAmount = player1Health / 100;

                if (player1Health <= 0)
                {

                    Debug.Log("Oyuncu 1 yenildi");

                }
               
                break;
            case 2:
                player2Health -= hitPower;

                player2HealthBar.fillAmount = player2Health / 100;

                if (player2Health <= 0)
                {

                    Debug.Log("Oyuncu 2 yenildi");

                }
                break;

        }

    }
}
