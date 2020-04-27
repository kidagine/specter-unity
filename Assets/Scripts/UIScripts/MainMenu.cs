using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EntityAudio))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private EntityAudio _mainMenuAudio = default;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame()
    {
        _animator.enabled = true;
        _mainMenuAudio.Play("Click");
        _mainMenuAudio.FadeOut("EvangelionMusic");
        _animator.SetTrigger("Start");
    }

    public void ExitGame()
    {
        _mainMenuAudio.Play("Click");
        Application.Quit();
    }

    public void Hover()
    {
        _mainMenuAudio.Play("Hover");
    }

    public void NextSceneAnimationEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
