using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;
	public Button button;
	public Button SkipButton;
	public Animator animator;
	private int[] NumberChange;
	private int i=0;
	private int MaxDialogue = 0;
	private Queue<string> sentences;
	private Queue<string> names;
	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
		names = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		sentences = new Queue<string>();
		names = new Queue<string>();
		i = 0;
		dialogueText.gameObject.SetActive(true);
		nameText.gameObject.SetActive(true);
		animator.SetBool("IsOpen", true);
		button.gameObject.SetActive(true);
		SkipButton.gameObject.SetActive(true);
		NumberChange = dialogue.NumberChangeCharacter;
		MaxDialogue = dialogue.sentences.Length;
		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		foreach (string name in dialogue.names) { names.Enqueue(name); }
			nameText.text = names.Dequeue();
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}
		if (NumberChange.Length != 0)
		{
			if (sentences.Count == MaxDialogue - NumberChange[i]) { nameText.text = names.Dequeue(); if (i+1 < NumberChange.Length ) i++; }
		}
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		dialogueText.gameObject.SetActive(false);
		nameText.gameObject.SetActive(false);
		button.gameObject.SetActive(false);
		SkipButton.gameObject.SetActive(false);
	}

}


