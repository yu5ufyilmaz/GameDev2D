using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class GameController : MonoBehaviourPunCallbacks
{
    [Header("Player Health Settings")]
    public Image player1HealthBar;
    public Image player2HealthBar;
    private float player1Health = 100;
    private float player2Health = 100;

    private bool isGameStarted = false;
    private int createCount = 0;
    private int rewardLimit = 4;
    private float createInterval = 15f;

    public GameObject[] dots;

    private void Start()
    {
        StartCoroutine(StartCreateRewards());
    }

    private IEnumerator StartCreateRewards()
    {
        createCount = 0;

        while (isGameStarted && createCount < rewardLimit)
        {
            yield return new WaitForSeconds(createInterval);

            if (createCount < rewardLimit)
            {
                int randomIndex = Random.Range(0, dots.Length);
                PhotonNetwork.Instantiate("Reward", dots[randomIndex].transform.position, dots[randomIndex].transform.rotation);
                createCount++;
            }
        }
    }

    [PunRPC]
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isGameStarted = true;
            StartCoroutine(StartCreateRewards());
        }
    }

    [PunRPC]
    public void HitDamage(int playerNumber, float hitPower)
    {
        if (playerNumber == 1)
        {
            player1Health -= hitPower;
            player1HealthBar.fillAmount = player1Health / 100f;

            if (player1Health <= 0)
            {
                HandleGameEnd(2); // Player 2 wins
            }
        }
        else if (playerNumber == 2)
        {
            player2Health -= hitPower;
            player2HealthBar.fillAmount = player2Health / 100f;

            if (player2Health <= 0)
            {
                HandleGameEnd(1); // Player 1 wins
            }
        }
    }

    private void HandleGameEnd(int winningPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Show end game panel
        GameObject endGamePanel = GameObject.FindGameObjectWithTag("EndGamePanel");
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            TextMeshProUGUI endGameInfo = endGamePanel.GetComponentInChildren<TextMeshProUGUI>();
            endGameInfo.text = winningPlayer == 1 ? "Player 1 Won" : "Player 2 Won";
        }

        // Trigger game end actions
        Winner(winningPlayer);
    }

    private void Winner(int winningPlayer)
    {
        // Inform both players about game result
        photonView.RPC("Result", RpcTarget.All, winningPlayer);

        // Prevent further game actions
        isGameStarted = false;
    }

    [PunRPC]
    public void FillHealth(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Health += 30f;
            if (player1Health > 100f)
                player1Health = 100f;

            player1HealthBar.fillAmount = player1Health / 100f;
        }
        else if (playerNumber == 2)
        {
            player2Health += 30f;
            if (player2Health > 100f)
                player2Health = 100f;

            player2HealthBar.fillAmount = player2Health / 100f;
        }
    }

    [PunRPC]
    public void MainMenu()
    {
        GameObject.FindGameObjectWithTag("ServerManager").GetComponent<ServerManager>().isWithButton = true;
        PhotonNetwork.LoadLevel(0);
    }

    [PunRPC]
    public void NormalExit()
    {
        PhotonNetwork.LoadLevel(0);
    }

    [PunRPC]
    public void Result(int winningPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>().Result(winningPlayer);
            GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>().Result(winningPlayer);
        }
    }
}
