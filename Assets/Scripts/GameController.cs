using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{


    [FormerlySerializedAs("Player1HealthBar")] [Header("Player Health Settings")]
    public Image player1HealthBar;
    float player1Health = 100;
    [FormerlySerializedAs("Player2HealthBar")] public Image player2HealthBar;
    float player2Health = 100;
    PhotonView pw;

    bool isWeStarted;
    int limit;
    float waitingTime;
    int createCount;
    public GameObject[] dots;
    GameObject player1;
    GameObject player2;

    bool isGameEnd = false;
    

    private void Start()
    {
        pw = GetComponent<PhotonView>();
        isWeStarted = false;
        limit = 4;
        waitingTime = 5f;
    }


    IEnumerator StartCreate()
    {
        createCount = 0;

        while (true && isWeStarted)
        {
            if (limit == createCount)
                isWeStarted = false;

            yield return new WaitForSeconds(15f);
            int resultValue = Random.Range(0, 7);
            PhotonNetwork.Instantiate("Reward", dots[resultValue].transform.position, dots[resultValue].transform.rotation, 0, null);
            createCount++;
        }

    }
    [PunRPC]
    public void StartGame() 
    {
        if (PhotonNetwork.IsMasterClient)
                isWeStarted = true;
                StartCoroutine(StartCreate());
       
    }

    [PunRPC]
    public void HitDamage(int criterion,float hitPower)
    {

        switch (criterion) 
        {

            case 1:
                
                    player1Health -= hitPower;

                    player1HealthBar.fillAmount = player1Health / 100;

                    if (player1Health <= 0)
                    {


                    foreach (GameObject myObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (myObject.gameObject.CompareTag("EndGamePanel"))
                        {
                            myObject.gameObject.SetActive(true);
                            GameObject.FindWithTag("EndGameInfo").GetComponent<TextMeshProUGUI>().text= "2. Player Won";
                            

                        }


                    }

                    Winner(2);                 

                     }
               
                break;
            case 2:                

                    player2Health -= hitPower;

                    player2HealthBar.fillAmount = player2Health / 100;

                    if (player2Health <= 0)
                    {

                    foreach (GameObject objem in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (objem.gameObject.CompareTag("EndGamePanel"))
                        {
                            objem.gameObject.SetActive(true);
                            GameObject.FindWithTag("EndGameInfo").GetComponent<TextMeshProUGUI>().text = "1.Player Won";


                        }


                    }

                    Winner(1);
                 
                    
                     }
                break;

        }

    }
    public void MainMenu()
    {
        GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().isWithButton = true;
        PhotonNetwork.LoadLevel(0);
       
    }
    public void NormalExit()
    {
       
        PhotonNetwork.LoadLevel(0);

    }


    void Winner(int value)
    {

        if (!isGameEnd)
        {
            GameObject.FindWithTag("Player1").GetComponent<Player>().Result(value);
            GameObject.FindWithTag("Player2").GetComponent<Player>().Result(value);
            isGameEnd = true;
        }

        

    }
    [PunRPC]
    public void FillHealth(int whichPlayer)
    {
        switch (whichPlayer)
        {

            case 1:
                player1Health += 30;

                if (player1Health > 100)
                {
                    player1Health = 100;
                    player1HealthBar.fillAmount = player1Health / 100;

                }
                else
                {
                    player1HealthBar.fillAmount = player1Health / 100;
                }
                break;
            
            case 2:
                player2Health += 30;

                if (player2Health > 100)
                {
                    player2Health = 100;
                    player2HealthBar.fillAmount = player2Health / 100;

                }
                else
                {
                    player2HealthBar.fillAmount = player2Health / 100;
                }
                break;

        }

    }
}