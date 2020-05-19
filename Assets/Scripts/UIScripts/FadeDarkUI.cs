using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeDarkUI : MonoBehaviour
{
	[SerializeField] Animator _animator;


	public void SetFade(bool state)
	{
		if (state)
		{
			_animator.SetTrigger("FadeIn");
		}
		else
		{
			_animator.SetTrigger("FadeOut");
		}
	}

	public void NextSceneAnimationEvent()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
