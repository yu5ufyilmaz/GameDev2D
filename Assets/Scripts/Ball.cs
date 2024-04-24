using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float hitPower = 20f;
    private int whoAmI; // 1 for Player1, 2 for Player2

    private GameObject gameController;
    private GameObject player;
    private PhotonView pv;
    private AudioSource destroySound;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        pv = GetComponent<PhotonView>();
        destroySound = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void TransferTag(string upcomingTag)
    {
        player = GameObject.FindWithTag(upcomingTag);

        if (upcomingTag == "Player1")
            whoAmI = 1;
        else if (upcomingTag == "Player2")
            whoAmI = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pv.IsMine)
        {
            switch (collision.gameObject.tag)
            {
                case "BoxInMid":
                    collision.gameObject.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.All, hitPower);
                    break;

                case "Player2Tower":
                case "Player2":
                    if (whoAmI != 2)
                        gameController.GetComponent<PhotonView>().RPC("HitDamage", RpcTarget.All, 2, hitPower);
                    break;

                case "Player1Tower":
                case "Player1":
                    if (whoAmI != 1)
                        gameController.GetComponent<PhotonView>().RPC("HitDamage", RpcTarget.All, 1, hitPower);
                    break;

                case "Ground":
                case "Wood":
                    // Damage handling or effects for hitting ground or wood (optional)
                    break;

                case "Reward":
                    gameController.GetComponent<PhotonView>().RPC("FillHealth", RpcTarget.All, whoAmI);
                    PhotonNetwork.Destroy(collision.gameObject);
                    break;

                case "Ball":
                    // Collision handling for another ball (optional)
                    break;
            }

            // Common actions for all collisions
            player.GetComponent<Player>().PowerPlay();
            PhotonNetwork.Instantiate("SmokeEffect", transform.position, transform.rotation);
            destroySound.Play();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
