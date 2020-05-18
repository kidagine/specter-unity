using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSplash : MonoBehaviour
{
	public void NextSceneAnimationEvent()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
