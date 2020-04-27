using UnityEngine;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private GameObject _deathObject = default;


    void Start()
    {
        if (GlobalSettings._hasDied)
        {
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
        CheckpointManager.Instance.Restart();
    }
}
