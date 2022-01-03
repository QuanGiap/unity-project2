using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxhealth = 90;
    public int currenthealth;
    public BossHealthBar BossHealthBar;
    public bool FirstHit = true;
    public bool SecondHit = true;
    public bool ThirdHit = true;
    public Vector3 SecondPosition;
    public Vector3 ThirdPosition;
    public Dialogue Dialogue1;
    public Dialogue Dialogue2;
    public Dialogue Dialogue3;
    public Animator Phase1;
    public Animator Phase2;
    public Animator Phase3;
    public GameObject ObjectPhase1;
    public GameObject ObjectPhase2;
    public GameObject ObjectPhase3;
    public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxhealth;
        BossHealthBar.SetMaxHealth(maxhealth);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currenthealth -= 30; BossHealthBar.SetHealth(currenthealth); 
            if (FirstHit) { collision.transform.position = SecondPosition;Phase1.SetBool("IsPhase", false); StartCoroutine(WaitDialogue(Dialogue1));StartCoroutine(NewPhase(ObjectPhase2,Phase2)); }
            else if (SecondHit)
            {
                collision.transform.position = ThirdPosition;
                Phase2.SetBool("IsPhase", false);
                StartCoroutine(WaitDialogue(Dialogue2));
                StartCoroutine(NewPhase(ObjectPhase3,Phase3));
            }
            else if (ThirdHit) { particle.Play(); StartCoroutine(WaitDialogue(Dialogue3)); Phase3.SetBool("IsPhase", false); ThirdHit = false; Destroy(gameObject,1); }
        } 
    }
    IEnumerator WaitDialogue(Dialogue dialogue)
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        if (FirstHit) { ObjectPhase1.gameObject.SetActive(false); FirstHit = false; }
        else if (SecondHit)
        {
            ObjectPhase2.SetActive(false); SecondHit = false;
        }
        else if (ThirdHit)
        {
            ObjectPhase3.SetActive(false);
        }
    }
    IEnumerator NewPhase(GameObject Phase,Animator animator)
    {
        yield return new WaitForSeconds(0.5f);
        Phase.SetActive(true);
        animator.SetBool("IsPhase", true);
    }
}
