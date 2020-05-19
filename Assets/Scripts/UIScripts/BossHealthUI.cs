using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
	[SerializeField] private Slider _healthSlider = default;


	public void SetHealth(int currentHealth)
	{
		_healthSlider.value = currentHealth;
	}
}
