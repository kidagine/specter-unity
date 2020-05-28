using System;
using UnityEngine;

public class PlayerCinematic : MonoBehaviour
{
	[SerializeField] private Animator _animator = default;
	[SerializeField] private GameObject _points = default;
	[SerializeField] private GameObject _dashEffectPrefab = default;
	[SerializeField] private PlayerUI _playerUI = default;
	[SerializeField] private Player _player = default;
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private PlayerInputSystem _playerInputSystem = default;
	[SerializeField] private TrapDoor _trapDoor = default;
	[SerializeField] private bool _hasEnteredRoom = default;

	public event Action ExitDoorEvent;


    void Awake()
    {
		_points.SetActive(false);
		PlayerCinematicPause(true);
		if (_hasEnteredRoom)
		{
			ExitDoor();
		}
    }

	public void EnterDoor()
	{
		_animator.SetTrigger("EnterDoor");
		_playerUI.FadeDarkUI.SetFade(true);
		_points.SetActive(false);
		Instantiate(_dashEffectPrefab, transform.position, transform.rotation);
	}

	public void ExitDoor()
	{
		GlobalSettings._hasDash = true;
		_playerUI.FadeDarkUI.SetFade(false);
		_trapDoor.CloseDoor();
		_animator.SetTrigger("ExitDoor");
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
		if (GlobalSettings._hasDash)
		{
			_points.SetActive(true);
		}
	}

	public void PlayerCinematicPause(bool state)
	{
		if (state)
		{
			_player.IsAttacking = true;
			_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
			_playerInputSystem.enabled = false;
		}
		else
		{
			_player.IsAttacking = false;
			_rigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
			_playerInputSystem.enabled = true;
		}
	}
}
