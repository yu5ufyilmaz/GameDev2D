using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    PhotonView pw;
    void Start()
    {
        pw = GetComponent<PhotonView>();
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {

        yield return new WaitForSeconds(10f);
        if(pw.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}