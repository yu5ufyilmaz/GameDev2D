using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float hitPower;
    int whoAmI;


    GameObject gameController;
    GameObject player;
    PhotonView pw;
    AudioSource destroySound;
    
    void Start()
    {
        hitPower = 20;
        gameController = GameObject.FindWithTag("GameController");
        pw = GetComponent<PhotonView>();
        destroySound = GetComponent<AudioSource>();
    }
    [PunRPC]
    public void TransferTag(string upcomingTag)
    {
        player = GameObject.FindWithTag(upcomingTag);

        if (upcomingTag == "Player1")
            whoAmI = 1;
        else
            whoAmI = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("BoxInMid"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.All, hitPower);
            player.GetComponent<Player>().PowerPlay();


            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);


        }
        if (collision.gameObject.CompareTag("Player2Tower") || collision.gameObject.CompareTag("Player2"))
        {
            if (whoAmI != 2)
            {
                gameController.GetComponent<PhotonView>().RPC("HitDamage", RpcTarget.All, 2, hitPower);

            }

            player.GetComponent<Player>().PowerPlay();


            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Player1Tower") || collision.gameObject.CompareTag("Player1"))
        {
            if (whoAmI != 1)
            {
                gameController.GetComponent<PhotonView>().RPC("HitDamage", RpcTarget.All, 1, hitPower);

            }
            player.GetComponent<Player>().PowerPlay();
            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }

        if (collision.gameObject.CompareTag("Ground"))
        {

            player.GetComponent<Player>().PowerPlay();
            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }

        if (collision.gameObject.CompareTag("Wood"))
        {

            player.GetComponent<Player>().PowerPlay();
            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }

        if (collision.gameObject.CompareTag("Reward"))
        {
            gameController.GetComponent<PhotonView>().RPC("FillHealth", RpcTarget.All, whoAmI);
            PhotonNetwork.Destroy(collision.transform.gameObject);
            player.GetComponent<Player>().PowerPlay();
            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }

        if (collision.gameObject.CompareTag("Ball"))
        {

            player.GetComponent<Player>().PowerPlay();

            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation, 0, null);
            destroySound.Play();
            if (pw.IsMine)
                PhotonNetwork.Destroy(gameObject);

        }



    }



}