using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private GameObject _deathObject = default;


    void Start()
    {
        if (GlobalSettings._hasDied)
        {
            GlobalSettings._hasDied = false;
            _animator.SetTrigger("SlideOut");
        }
    }

    public void SetDeath(bool state)
    {
        _animator.SetTrigger("SlideIn");
        if (state)
        {
            _deathObject.SetActive(state);
        }
    }

    public void Restart()
    {
        GlobalSettings._hasDied = true;
        SceneManager.LoadScene(2);
    }
}
