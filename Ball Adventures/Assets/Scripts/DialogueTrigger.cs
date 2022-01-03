using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;

    private void Start()
    {
      StartCoroutine(WaitDialogue());
    }

    IEnumerator WaitDialogue()
    { 
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue) ;
    }
    public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}
}
