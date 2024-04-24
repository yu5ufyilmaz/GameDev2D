using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject firstPanel;
    public GameObject secondPanel;
    public InputField username;
    public Text activeUsername;
    public TextMeshProUGUI[] statistic;
    public Text serverInfo;

    private void Start()
    {
        InitializePlayerPrefs();
    }

    private void InitializePlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("Username"))
        {
            // Yeni bir kullanıcı için varsayılan değerler ayarla
            PlayerPrefs.SetInt("TotalMatch", 0);
            PlayerPrefs.SetInt("Lose", 0);
            PlayerPrefs.SetInt("Win", 0);
            PlayerPrefs.SetInt("TotalScore", 0);

            // İlk paneli aktif hale getir ve kullanıcı adı girişi yap
            firstPanel.SetActive(true);
        }
        else
        {
            // Kayıtlı bir kullanıcı varsa ikinci paneli aktif hale getir
            secondPanel.SetActive(true);
            activeUsername.text = PlayerPrefs.GetString("Username");
        }

        // İstatistikleri güncelle
        UpdateStatistics();
    }

    public void SaveUsername()
    {
        // Kullanıcı adını kaydet
        string enteredUsername = username.text;
        if (string.IsNullOrEmpty(enteredUsername))
        {
            // Kullanıcı adı boşsa işlem yapma
            Debug.LogWarning("Kullanıcı adı boş olamaz!");
            return;
        }

        PlayerPrefs.SetString("Username", enteredUsername);

        // Panel geçişleri ve kullanıcı adı güncellemesi
        firstPanel.SetActive(false);
        secondPanel.SetActive(true);
        activeUsername.text = enteredUsername;
    }

    private void UpdateStatistics()
    {
        // Kayıtlı istatistikleri görüntüle
        statistic[0].text = PlayerPrefs.GetInt("TotalMatch", 0).ToString();
        statistic[1].text = PlayerPrefs.GetInt("Lose", 0).ToString();
        statistic[2].text = PlayerPrefs.GetInt("Win", 0).ToString();
        statistic[3].text = PlayerPrefs.GetInt("TotalScore", 0).ToString();
    }

    public void ResetPlayerPrefs()
    {
        // Oyun istatistiklerini sıfırla (test amaçlı)
        PlayerPrefs.DeleteAll();
        InitializePlayerPrefs(); // Yeniden başlat
    }
}
