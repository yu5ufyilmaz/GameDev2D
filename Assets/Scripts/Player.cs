using Photon.Pun;
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
    float shootingDirection;
     

   [Header("Power Bar Settings")]
    Image powerBar;
    float powerScore;    
    bool isEnd=false;
    Coroutine powerLoop;

    PhotonView pw;
    bool isFireActive=false;
    void Start()
    {      

        pw = GetComponent<PhotonView>();

        if (pw.IsMine)
        {
            powerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>();
            if (PhotonNetwork.IsMasterClient)
            {
                gameObject.tag = "Player1";
                transform.position = GameObject.FindWithTag("CreatedDots1").transform.position;
                transform.rotation = GameObject.FindWithTag("CreatedDots1").transform.rotation;
                shootingDirection = 2f;                
            }
            else
            {
                gameObject.tag = "Player2";
                transform.position = GameObject.FindWithTag("CreatedDots2").transform.position;
                transform.rotation = GameObject.FindWithTag("CreatedDots2").transform.rotation;
                shootingDirection = -2f;
                
            }

        }
        InvokeRepeating("IsGameStarted", 0, .5f);

    }
    public void IsGameStarted()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (pw.IsMine)
            {
                powerLoop = StartCoroutine(RunPowerBar());
                CancelInvoke("IsGameStarted");

            }
           

        }else
        {
            StopAllCoroutines();
        }
    }
    IEnumerator RunPowerBar()
    {
        powerBar.fillAmount = 0;
        isEnd = false;
        isFireActive = true;

        while (true)
        {
            if (powerBar.fillAmount < 1 && !isEnd)
            {
                powerScore = 0.01f;
                powerBar.fillAmount += powerScore;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

            }else
            {
                isEnd = true;
                powerScore = 0.01f;
                powerBar.fillAmount -= powerScore;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if (powerBar.fillAmount==0)
                {
                    isEnd = false;

                }

            }


        }

    }

    
    
    void Update()
    {
        
        if (pw.IsMine)
        {
            if (Input.touchCount > 0 && isFireActive) 
            {
               
                PhotonNetwork.Instantiate("BlowEffect", ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation, 0, null);
                ballThrowSound.Play();
                GameObject ballObject = PhotonNetwork.Instantiate("Ball", ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation, 0, null);


                ballObject.GetComponent<PhotonView>().RPC("TransferTag",RpcTarget.All, gameObject.tag);

                Rigidbody2D rg = ballObject.GetComponent<Rigidbody2D>();
                rg.AddForce(new Vector2(shootingDirection, 0f) * powerBar.fillAmount * 12f, ForceMode2D.Impulse);
                isFireActive = false;
                StopCoroutine(powerLoop);
                
            }

        }

       

        
    }


    public void PowerPlay()
    {
        powerLoop = StartCoroutine(RunPowerBar());
    }
   
    public void Result(int value)
    {
       
        if (pw.IsMine)
        {

            if (PhotonNetwork.IsMasterClient)
                {

                if (value==1)
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);

                }

            }
            else
            {
                
                if (value == 2)
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Galibiyet") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
                    PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);

                }


            }
        }
        Time.timeScale = 0;
    }
}
