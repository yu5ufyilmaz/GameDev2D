using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject ball;
    public GameObject ballSpawnPoint;
    public ParticleSystem ballThrowEffect;
    public AudioSource ballThrowSound;

    private Image _powerBar;

    private PhotonView _photonView;
    private bool _isFireActive = true;
    private Coroutine _powerLoop;

    private float _shootingDirection;
    private bool _isEnd;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
        {
            SetupPlayer();
            InvokeRepeating("CheckGameStart", 0, 0.5f);
        }
    }

    private void SetupPlayer()
    {
        _powerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>();

        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.tag = "Player1";
            SetPlayerPositionAndDirection("CreatedDots1", 2f);
        }
        else
        {
            gameObject.tag = "Player2";
            SetPlayerPositionAndDirection("CreatedDots2", -2f);
        }
    }

    private void SetPlayerPositionAndDirection(string dotsTag, float direction)
    {
        Transform dotsTransform = GameObject.FindWithTag(dotsTag).transform;
        transform.position = dotsTransform.position;
        transform.rotation = dotsTransform.rotation;
        _shootingDirection = direction;
    }

    private void CheckGameStart()
    {
        if (!_isEnd && _powerBar.fillAmount < 1 && PhotonNetwork.PlayerList.Length == 2 && _photonView.IsMine)
        {
            if (_powerLoop == null)
            {
                _powerLoop = StartCoroutine(RunPowerBar());
            }
        }
    }

    private IEnumerator RunPowerBar()
    {
        _powerBar.fillAmount = 0;
        _isEnd = false;
        _isFireActive = true;

        while (!_isEnd && _powerBar.fillAmount < 1)
        {
            _powerBar.fillAmount += 0.01f;
            yield return new WaitForSeconds(0.001f);
        }

        if (_powerBar.fillAmount == 0)
        {
            _isEnd = false;
        }
    }

    private void Update()
    {
        if (_photonView.IsMine && _isFireActive && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
        {
            FireBall();
        }
    }

    private void FireBall()
    {
        PhotonNetwork.Instantiate("BlowEffect", ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
        ballThrowSound.Play();
        GameObject ballObject = PhotonNetwork.Instantiate("Ball", ballSpawnPoint.transform.position, ballSpawnPoint.transform.rotation);
        ballObject.GetComponent<PhotonView>().RPC("TransferTag", RpcTarget.AllBuffered, gameObject.tag);

        Rigidbody2D rg = ballObject.GetComponent<Rigidbody2D>();
        rg.AddForce(new Vector2(_shootingDirection, 0f) * _powerBar.fillAmount * 12f, ForceMode2D.Impulse);

        _isFireActive = false;
        StopCoroutine(_powerLoop);
    }

    public void PowerPlay()
    {
        _powerLoop = StartCoroutine(RunPowerBar());
    }

    public void Result(int value)
    {
        if (_photonView.IsMine)
        {
            PlayerPrefs.SetInt("TotalMatch", PlayerPrefs.GetInt("TotalMatch") + 1);

            if (PhotonNetwork.IsMasterClient)
            {
                if (value == 1)
                {
                    PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);
                }
            }
            else
            {
                if (value == 2)
                {
                    PlayerPrefs.SetInt("Win", PlayerPrefs.GetInt("Win") + 1);
                    PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + 150);
                }
                else
                {
                    PlayerPrefs.SetInt("Lose", PlayerPrefs.GetInt("Lose") + 1);
                }
            }
        }

        // Oyun zamanını durdurma yerine farklı bir yöntem kullanılabilir
    }
}
