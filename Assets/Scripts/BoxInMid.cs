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
    private void Start()
    {
        gameController = GameObject.FindWithTag("GameControl");
    }
    public void TakeHit(float hitPower)
    {
        health -= hitPower;

        healthBar.fillAmount = health / 100; // 0.9

        if (health<=0)
        {

            gameController.GetComponent<GameController>().CreateEffects(2,gameObject);
            Destroy(gameObject);

        }else
        {
            StartCoroutine(ActivateCanvas());

        }

        
    }
    IEnumerator ActivateCanvas()
    {
        if (!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2);
            healthCanvas.SetActive(false);
        }

    }
 
}
