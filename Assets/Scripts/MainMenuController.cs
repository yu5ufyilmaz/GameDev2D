using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject firstPanel;
    public GameObject secondPanel;
    public InputField username;
    public Text activeUsername;
    public TextMeshProUGUI[] statistic;
    public Text serverInfo;
    GameObject randomJoin;
    GameObject createRoom;

    void Start()
    {
        

        if (!PlayerPrefs.HasKey("Username"))
        {
            PlayerPrefs.SetInt("TotalMatch", 0);
            PlayerPrefs.SetInt("Lose", 0);
            PlayerPrefs.SetInt("Win", 0);
            PlayerPrefs.SetInt("TotalScore", 0);

            firstPanel.SetActive(true);
            EnterValues();

        }
        else
        {
            secondPanel.SetActive(true);
            activeUsername.text = PlayerPrefs.GetString("Usernam");
            EnterValues();
        }
        
    }

    public void SaveUsername()
    {
       
        PlayerPrefs.SetString("Username", username.text);

        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
        activeUsername.text = username.text;
        randomJoin = GameObject.FindWithTag("RandomJoin");
        createRoom = GameObject.FindWithTag("CreateRoom");
        randomJoin.GetComponent<Button>().interactable = true;
        createRoom.GetComponent<Button>().interactable = true;

    }

    void EnterValues()
    {
        statistic[0].text = PlayerPrefs.GetInt("TotalMatch").ToString();
        statistic[1].text = PlayerPrefs.GetInt("Lose").ToString();
        statistic[2].text = PlayerPrefs.GetInt("Win").ToString();
        statistic[3].text = PlayerPrefs.GetInt("TotalScore").ToString();
    }
}