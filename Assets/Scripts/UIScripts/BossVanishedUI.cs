using UnityEngine;

public class BossVanishedUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private EntityAudio _bossUIAudio = default;


    public void ShowVanished()
    {
        _bossUIAudio.Play("BossVanished");
        _animator.SetTrigger("ShowVanished");
    }
}
