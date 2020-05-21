using System;
using UnityEngine;

public class PlayerCinematic : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private GameObject _points;
	[SerializeField] private PlayerUI _playerUI;
	[SerializeField] private Player _player;
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private TrapDoor _trapDoor;
	[SerializeField] private bool _hasEnteredRoom;

	public event Action ExitDoorEvent;


    void Awake()
    {
		_points.SetActive(false);
		PlayerCinematicPause(true);
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
		PlayerCinematicPause(false);
		_points.SetActive(true);
		ExitDoorEvent?.Invoke();
	}

	public void StartPlayerCinematicIntro()
	{
		_animator.SetTrigger("WakeUp");
	}

	public void WokeUpAnimationEvent()
	{
		PlayerCinematicPause(false);
	}

	public void PlayerCinematicPause(bool state)
	{
		if (state)
		{
			_player.IsAttacking = true;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
			_playerMovement.LockMovement(true);
		}
		else
		{
			_player.IsAttacking = false;
			_rigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
			_playerMovement.LockMovement(false);
		}
	}
}
