using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerDialogueSystem : MonoBehaviour
{
	[SerializeField] private PlayerUI _playerUI = default;
	[SerializeField] private PlayerMovement _playerMovement = default;
	private Dialogue _dialogue;
	private bool _isOnDialogueTrigger;
	private bool _hasDialogueStarted;
	private bool _hasDialogueBeenSaid;


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out DialogueTrigger dialogueTrigger))
		{
			_isOnDialogueTrigger = true;
			_dialogue = dialogueTrigger.GetDialogue();
			Vector2 dialoguePromptPosition = dialogueTrigger.GetDialoguePromptPosition();
			_playerUI.DialogueUI.SetPrompt(true, dialoguePromptPosition);
		}
	}

	public void Talk()
	{
		if (_isOnDialogueTrigger)
		{
			if (!_hasDialogueStarted && !_hasDialogueBeenSaid)
			{
				_playerMovement.LockMovement(true);
				_playerUI.DialogueUI.StartDialogue();
				DialogueManager.Instance.StartDialogue(_dialogue, 1);
				_hasDialogueStarted = true;
				_hasDialogueBeenSaid = true;
			}
			else
			{
				DialogueManager.Instance.NextSentence();
				if (DialogueManager.Instance.HasDialogueEnded())
				{
					_playerMovement.LockMovement(false);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out DialogueTrigger dialogueTrigger))
		{
			_isOnDialogueTrigger = false;
			_hasDialogueStarted = false;
			_playerUI.DialogueUI.SetPrompt(false);
		}
	}
}
