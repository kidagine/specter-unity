using UnityEngine;

public class BossVanishedUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;


    public void ShowVanished()
    {
        _animator.SetTrigger("ShowVanished");
    }
}
