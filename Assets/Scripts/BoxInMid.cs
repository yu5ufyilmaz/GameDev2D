using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoxInMid : MonoBehaviour
{
    float health = 100;
    public GameObject healthCanvas;
    public Image healthBar;    

    GameObject gameController;
    PhotonView pw;
    AudioSource crateBreakSound;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        pw = GetComponent<PhotonView>();
        crateBreakSound = GetComponent<AudioSource>();
    }
    [PunRPC]
    public void GetHit(float hitPower)
    {

        if (pw.IsMine)
        {

            health -= hitPower;

            healthBar.fillAmount = health / 100; // 0.9

            if (health <= 0)
            {

                // gameController.GetComponent<GameController>().CreateSound(2, gameObject);

                PhotonNetwork.Instantiate("CrateBreakEffect", transform.position, transform.rotation, 0, null);
                crateBreakSound.Play();
                PhotonNetwork.Destroy(gameObject);

            }
            else
            {
                StartCoroutine(RemoveCanvas());

            }

        }
           

        
    }


    IEnumerator RemoveCanvas()
    {
        if (!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2);
            healthCanvas.SetActive(false);
        }

    }
 
}