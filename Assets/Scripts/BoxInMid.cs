using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxInMid : MonoBehaviour
{
    private float health = 100f;
    public GameObject healthCanvas;
    public Image healthBar;

    private PhotonView photonView;
    private AudioSource crateBreakSound;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        crateBreakSound = GetComponent<AudioSource>();

        // Sağlık çubuğunu başlangıçta güncelle
        UpdateHealthBar();
    }

    [PunRPC]
    public void GetHit(float hitPower)
    {
        if (photonView.IsMine)
        {
            // Hasar al ve sağlık çubuğunu güncelle
            health -= hitPower;
            UpdateHealthBar();

            if (health <= 0f)
            {
                // Kutu yok edilir
                DestroyBox();
            }
            else
            {
                // Hasar aldıktan sonra sağlık çubuğunu göster
                ShowHealthCanvas();
            }
        }
    }

    private void UpdateHealthBar()
    {
        // Sağlık çubuğunu güncelle
        healthBar.fillAmount = health / 100f;
    }

    private void DestroyBox()
    {
        if (photonView.IsMine)
        {
            // Kutuyu yok etme efektini ve sesini oynat
            PlayDestroyEffects();

            // Kutuyu ağ üzerinden yok et
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void PlayDestroyEffects()
    {
        // Kırılma efektini ağ üzerinden oluştur
        PhotonNetwork.Instantiate("CrateBreakEffect", transform.position, transform.rotation);

        // Ses efektini oynat
        if (crateBreakSound != null)
            crateBreakSound.Play();
    }

    private void ShowHealthCanvas()
    {
        // Sağlık çubuğunu göster ve belirli bir süre sonra gizle
        if (healthCanvas != null && !healthCanvas.activeSelf)
        {
            healthCanvas.SetActive(true);
            StartCoroutine(HideHealthCanvas());
        }
    }

    private IEnumerator HideHealthCanvas()
    {
        // Belirli bir süre sonra sağlık çubuğunu gizle
        yield return new WaitForSeconds(2f);
        if (healthCanvas != null && healthCanvas.activeSelf)
        {
            healthCanvas.SetActive(false);
        }
    }
}
