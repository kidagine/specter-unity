using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private GameObject _dialoguePromptButton = default;
	[SerializeField] private Dialogue _dialogue = default;
	[SerializeField] private int _indexSkipTo = default;
	private bool _hasDialogueStarted;
	private bool _hasDialogueBeenSaid;


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out Player player))
		{
			//_dialoguePromptButton.SetActive(true);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out Player player))
		{
			if (Input.GetKey(KeyCode.K) && !_hasDialogueStarted && !_hasDialogueBeenSaid)
			{
				DialogueManager.Instance.StartDialogue(_dialogue, 1);
				_hasDialogueStarted = true;
				_hasDialogueBeenSaid = true;
				//_dialoguePromptButton.SetActive(false);
			}
			else if (Input.GetKey(KeyCode.K) && !_hasDialogueStarted && _hasDialogueBeenSaid)
			{
				DialogueManager.Instance.StartDialogue(_dialogue, _indexSkipTo);
				_hasDialogueStarted = true;
				//_dialoguePromptButton.SetActive(false);
			}
			else if (!_hasDialogueStarted)
			{
				//_dialoguePromptButton.SetActive(true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out Player player))
		{
			//_dialoguePromptButton.SetActive(false);
			_hasDialogueStarted = false;
		}
	}
}
