using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
	[SerializeField] private Slider _healthSlider = default;
	[SerializeField] private Animator _animator = default;


	public void ShowHealth(bool state)
	{
		_animator.SetBool("ShowHealth", state);
	}

	public void SetHealth(int currentHealth)
	{
		_healthSlider.value = currentHealth;
	}
}
