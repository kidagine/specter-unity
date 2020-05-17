using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EntityAudio))]
public class DialogueManager : MonoBehaviour
{
	public GameObject dialoguePane;
	public GameObject dialogueArrow;
	public TextMeshProUGUI textDisplay;
	[SerializeField] private Animator _dialoguePaneAnimator;
	[SerializeField] private TextMeshProUGUI _nameDisplay;
	[SerializeField] private EntityAudio _dialogueAudio;
	[SerializeField] private ItemUI _itemUI;
	public Queue<string> sentences = new Queue<string>();
	public float typingSpeed;
	private bool isDialogueTyping;
	private bool sentenceFinished;
	private bool _hasDialogueEnded;
	private string currentSentence;
	private string sentence;

	public static DialogueManager Instance { get; private set; }


	void Awake()
	{
		CheckInstance();
	}


	public bool HasDialogueEnded()
	{
		return _hasDialogueEnded;
	}

	public void NextSentence()
	{
		if (!_hasDialogueEnded)
		{
			if (sentenceFinished == true)
			{
				isDialogueTyping = true;
				sentenceFinished = false;
				dialogueArrow.SetActive(false);
				DisplayNextSentence(1);
			}
			else if (isDialogueTyping == true)
			{
				StopAllCoroutines();
				isDialogueTyping = false;
				sentenceFinished = true;
				textDisplay.text = "";
				textDisplay.text = currentSentence;
				dialogueArrow.SetActive(true);
			}
		}
	}

	private void CheckInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void StartDialogue(Dialogue dialogue, int indexSkipToPass)
	{
		isDialogueTyping = true;
		dialoguePane.SetActive(true);
		_dialoguePaneAnimator.SetTrigger("Open");

		sentences.Clear();
		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		_nameDisplay.text = dialogue.name;
		DisplayNextSentence(indexSkipToPass);
	}

	public void DisplayNextSentence(int indexSkipTo)
	{
		if (sentences.Count == 0)
		{
			EndDialog();
			return;
		}
		for (int i = 0; i < indexSkipTo; i++)
		{
			sentence = sentences.Dequeue();
		}
		currentSentence = sentence;
		StopAllCoroutines();
		StartCoroutine(Type(sentence));
	}

	IEnumerator Type(string sentence)
	{
		textDisplay.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			_dialogueAudio.Play("Typing");
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
		sentenceFinished = true;
		dialogueArrow.SetActive(true);
	}

	public void EndDialog()
	{
		_hasDialogueEnded = true;
		textDisplay.text = "";
		_dialoguePaneAnimator.SetTrigger("Close");
		StartCoroutine(DisablePane());
	}

	IEnumerator DisablePane()
	{
		yield return new WaitForSeconds(0.25f);
		dialoguePane.SetActive(false);
		if (!GlobalSettings._hasDash)
		{
			_itemUI.ShowItem(true);
		}
	}
}
