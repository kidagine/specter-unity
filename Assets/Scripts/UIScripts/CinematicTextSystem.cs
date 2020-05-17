using System.Collections;
using TMPro;
using UnityEngine;

public class CinematicTextSystem : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _cinematicDisplayText;
	[SerializeField] private PlayerCinematic _playerCinematic;
	[SerializeField] private CinematicText[] _cinematicTexts;


    void Start()
    {
		StartCoroutine(ChangeTextCoroutine());
    }

	IEnumerator ChangeTextCoroutine()
	{
		for (int i = 0; i < _cinematicTexts.Length; i++)
		{
			yield return new WaitForSeconds(_cinematicTexts[i].timeBetween);
			_cinematicDisplayText.text = _cinematicTexts[i].sentence;
		}
	}

	public void StartWakeUpAnimationEvent()
	{
		_playerCinematic.StartPlayerCinematicIntro();
	}
}
