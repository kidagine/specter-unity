using UnityEngine;

public class TrapDoor : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Vector2 _promptOffset;


	public void OpenDoor()
	{
		_animator.SetTrigger("Open");
	}

	public Vector2 GetInteractPromptPosition()
	{
		Vector2 interactPromptPosition = new Vector2(transform.position.x + _promptOffset.x, transform.position.y + _promptOffset.y);
		return interactPromptPosition;
	}
}
