using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{

    GameObject serverInfo;
    GameObject saveName;
    GameObject randomJoin;
    GameObject createRoom;
    public bool isWithButton;
    void Start()
    {
                
        serverInfo = GameObject.FindWithTag("ServerInfo");
        saveName = GameObject.FindWithTag("SaveNameButton");
        randomJoin = GameObject.FindWithTag("RandomJoin");
        createRoom = GameObject.FindWithTag("CreateRoom");
        
        PhotonNetwork.ConnectUsingSettings();

        DontDestroyOnLoad(gameObject);
    }


    public override void OnConnectedToMaster()
    {
        serverInfo.GetComponent<Text>().text = "Connected to Server";
       
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        serverInfo.GetComponent<Text>().text = "Connected to Lobby";
        

        if (!PlayerPrefs.HasKey("Username"))
        {
            saveName.GetComponent<Button>().interactable = true;
        }else
        {
            randomJoin.GetComponent<Button>().interactable = true;
            createRoom.GetComponent<Button>().interactable = true;
        }
           
           
    }

    public void RandomJoin()
    {
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.JoinRandomRoom();

    }
    public void CreateRoom()
    {
        PhotonNetwork.LoadLevel(1);
        string roomName = Random.Range(0, 9964124).ToString();
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {

        InvokeRepeating("CheckInfos", 0, 1f);
        GameObject myObjects = PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity,0,null);
        myObjects.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("Username");

        if (PhotonNetwork.PlayerList.Length==2)
        {
            myObjects.gameObject.tag = "Player2";
            GameObject.FindWithTag("GameController").gameObject.GetComponent<PhotonView>().RPC("StartGame", RpcTarget.All);
        }
       
    }

    public override void OnLeftRoom()

    {
        if (isWithButton)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }else
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
            //  Debug.Log("Sen Çıktın");
              PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);
             PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);
        }
    }

    public override void OnLeftLobby()

    {
        // lobiden

    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // bir oyuncu girdiyse

    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)

    {
        if (isWithButton)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {
            Time.timeScale = 1;
          PhotonNetwork.ConnectUsingSettings();
          PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);       
          PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
          PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 150);
        }



        // Debug.Log("Rakip Çıktı");
        InvokeRepeating("CheckInfos", 0, 1f);

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
       {
        serverInfo.GetComponent<Text>().text = "Couldn't Join Room";

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    
    {
        serverInfo.GetComponent<Text>().text = "Couldn't Join a Random Room";

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        serverInfo.GetComponent<Text>().text = "Room couldn't create";

    }


    void CheckInfos()
    {

        if (PhotonNetwork.PlayerList.Length==2)
        {
            GameObject.FindWithTag("WaitingPlayers").SetActive(false);
            GameObject.FindWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke("CheckInfos");           
        }
        else
        {

            GameObject.FindWithTag("WaitingPlayers").SetActive(true);
            GameObject.FindWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text = ".......";
        }

       

    }


}
