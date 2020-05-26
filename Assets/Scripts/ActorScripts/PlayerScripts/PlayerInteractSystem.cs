using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteractSystem : MonoBehaviour
{
	[SerializeField] private Animator _animator = default;
	[SerializeField] private GameObject _dashEffectPrefab = default;
	[SerializeField] private PlayerCinematic _playerCinematic = default;
	[SerializeField] private PlayerUI _playerUI = default;
	[SerializeField] private PlayerMovement _playerMovement = default;
	private TrapDoor _trapDoor;
	private Lever _lever;
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
		if (other.gameObject.TryGetComponent(out TrapDoor trapDoor))
		{
			_trapDoor = trapDoor;
			Vector2 promptPosition = new Vector2(transform.position.x, transform.position.y - 2.5f);
			_playerUI.InteractUI.SetPrompt(true, promptPosition);
		}
		if (other.gameObject.TryGetComponent(out Lever lever))
		{
			_lever = lever;
			Vector2 promptPosition = _lever.GetPromptPosition();
			_playerUI.InteractUI.SetPrompt(true, promptPosition);
		}
	}

	public void Interact()
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
		if (_trapDoor != null)
		{
			_playerCinematic.EnterDoor();
			_trapDoor.OpenDoor();
			_trapDoor = null;
		}
		if (_lever != null)
		{
			_lever.OpenGate();
			_lever = null;
		}
		_playerUI.InteractUI.SetPrompt(false);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out DialogueTrigger dialogueTrigger))
		{
			_isOnDialogueTrigger = false;
			_hasDialogueStarted = false;
			_playerUI.DialogueUI.SetPrompt(false);
		}
		if (other.gameObject.TryGetComponent(out TrapDoor trapDoor))
		{
			_playerUI.InteractUI.SetPrompt(false);
		}
		if (other.gameObject.TryGetComponent(out Lever lever))
		{
			_playerUI.InteractUI.SetPrompt(false);
		}
	}
}
