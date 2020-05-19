using UnityEngine;

public class PlayerCinematic : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private GameObject _points;
	[SerializeField] private PlayerUI _playerUI;
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private TrapDoor _trapDoor;
	[SerializeField] private bool _hasEnteredRoom;

    void Awake()
    {
		_points.SetActive(false);
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		_playerMovement.enabled = false;
		if (_hasEnteredRoom)
		{
			GlobalSettings._hasDash = true;
			_playerUI.FadeDarkUI.SetFade(false);
			_trapDoor.CloseDoor();
			_animator.SetTrigger("ExitDoor");
		}
    }

	public void ExitDoorAnimationEvent()
	{
		_rigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		_playerMovement.enabled = true;
		_points.SetActive(true);
	}

	public void StartPlayerCinematicIntro()
	{
		_animator.SetTrigger("WakeUp");
	}

	public void WokeUpAnimationEvent()
	{
		_rigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		_playerMovement.enabled = true;
	}
}
