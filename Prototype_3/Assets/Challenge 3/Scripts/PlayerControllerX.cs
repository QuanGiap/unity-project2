using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    public float stableForce;
    private float gravityModifier = 2f;
    private Rigidbody playerRb;
    public GameObject PowerUpIndicator;
    public GameObject PowerUpIndicator2;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    public Button RestartButton;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI GoldRushText;
    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    private double timer=0;
    public double InvicibleTime;
    private SpawnManagerX SpawnManagerXScript;
    private SystemManager SystemManager;
    private int points=0;
    public bool IsDoublePoint = false;
    public bool IsInvicible = false;
    public bool IsGoldRush = false;
    public int PowerUpDuration= 10;
    // Start is called before the first frame update
    void Start()
    {
        SystemManager = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        SpawnManagerXScript = GameObject.Find("SpawnManager").GetComponent<SpawnManagerX>();
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        PowerUpIndicator.transform.position = transform.position;
        PowerUpIndicator2.transform.position = transform.position + new Vector3(0,1.5f);
        if (SystemManager.IsGameStart == true)
        {    //the balloon stay there for short of time
            if (timer < InvicibleTime)
            {
                transform.position = new Vector3(-5.7f, 9.64f, 0);
                timer += Time.deltaTime;
                playerRb.AddForce(Vector3.up * stableForce);
                if (Input.GetKeyDown(KeyCode.Space)) timer = InvicibleTime;
            }
            // While space is pressed and player is low enough, float up
            if (Input.GetKey(KeyCode.Space) && !gameOver)
            {
                playerRb.AddForce(Vector3.up * floatForce);
            }
            if (transform.position.y > 13.9)
            {
                playerRb.AddForce(Vector3.down * floatForce);
            }
            if (transform.position.y < 0) { playerRb.AddForce(Vector3.up * floatForce); }
            if (transform.position.y < -3 && gameOver) { Destroy(gameObject); }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            if (IsInvicible == false)
            {
                gameOver = true;
                GameOverText.gameObject.SetActive(true);
                RestartButton.gameObject.SetActive(true);
                Physics.gravity /= gravityModifier;
            }
        }
        //Power Up pick up
        if (other.gameObject.CompareTag("Point"))      //Double Point
        {
            IsDoublePoint = true;
            PowerUpIndicator.SetActive(true);
            StartCoroutine(PowerUpCountDown(1));
        }
        if (other.gameObject.CompareTag("Block"))     //Invicible
        {
            IsInvicible = true;
            PowerUpIndicator2.SetActive(true);
            StartCoroutine(PowerUpCountDown(2));
        }
        if (other.gameObject.CompareTag("GoldRush"))     //Gold Rush
        {
            IsGoldRush = true;
            GoldRushText.gameObject.SetActive(true);
            SpawnManagerXScript.spawnInterval = 0.5f;
            StartCoroutine(PowerUpCountDown(3));

        }
        // if player collides with money, fireworks
        if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            if (IsDoublePoint == true) points += 20; else points += 10;
            SystemManager.UpdateScore(points);
        }
        Destroy(other.gameObject);
    }
    IEnumerator PowerUpCountDown(int PowerType)
    {
        yield return new WaitForSeconds(PowerUpDuration);
        if (PowerType == 1) DoublePointCountDown();
        else if (PowerType == 2) InvicibleCountDown();
        else if (PowerType == 3) GoldRushCountDown();
    }
    void DoublePointCountDown()
    {
        PowerUpIndicator.SetActive(false);
        IsDoublePoint = false;
    }

    void InvicibleCountDown()
    {
        PowerUpIndicator2.SetActive(false);
        IsInvicible = false;
    }
    void GoldRushCountDown()
    {
        SpawnManagerXScript.spawnInterval = 1.5f;
        GoldRushText.gameObject.SetActive(false);
        IsGoldRush = false;
    }

    public void UseGravity()
    {
        playerRb.useGravity = true; Physics.gravity *= gravityModifier;
    }

    public void UnUseGravity()
    {  playerRb.useGravity = true;  }
}
