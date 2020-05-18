using UnityEngine;

public class InteractUI : MonoBehaviour
{
	[SerializeField] private Camera _camera = default;
	[SerializeField] private GameObject _promptObject = default;
	[SerializeField] private Transform _promptPoint = default;


	public void SetPrompt(bool state, Vector2 promptPosition = default)
	{
		if (state)
		{
			_promptObject.SetActive(true);
			PositionPrompt(promptPosition);
		}
		else
		{
			_promptObject.SetActive(false);
		}
	}

	private void PositionPrompt(Vector2 promptPosition)
	{
		Vector2 promptPositionWorldSpace = _camera.WorldToScreenPoint(promptPosition);
		_promptPoint.position = promptPositionWorldSpace;
	}
}
