using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerControl : MonoBehaviour
{private Rigidbody playerRb;
    private Animator playerAnim;
    public float jumpForce;
    public float startfall;
    public float gravityModifier;
    public bool isOnground = true;
    public bool gameOver = false;
    public ParticleSystem explosion;
    public ParticleSystem dirt;
    public AudioClip jumpsound;
    public AudioClip crashsound;
    private AudioSource playerAudio;
    int a = 0;
    private float y1=0;
    public float y2=0;
    // Start is called before the first frame update
     void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        if (Input.GetKeyDown(KeyCode.Space) && isOnground && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnground = false;
            dirt.Stop();
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpsound, 0.7f);
        }
        if (y2 < y1) {playerAnim.SetBool("Grounded",false);} else { playerAnim.SetBool("Grounded", true); }
        y1 = y2;
        y2 = transform.position.y;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) { isOnground = true; dirt.Play(); } else if (collision.gameObject.CompareTag("Obstacle")) { gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            dirt.Stop();
            playerAudio.PlayOneShot(crashsound, 1.0f);
            if (a == 0) 
            { explosion.Play(); a = 1;
            }
        }
    }
}
