using UnityEngine;

public class PlayerCinematic : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private GameObject _points;
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private PlayerAim _playerAim;


    void Awake()
    {
		_points.SetActive(false);
		_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		_playerMovement.enabled = false;
		//_playerAim.enabled = false;
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
