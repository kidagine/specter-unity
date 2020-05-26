using UnityEngine;

public class BossTitleUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private GameObject _bossCamera = default;
    [SerializeField] private BossCinematicSystem _bossCinematicSystem = default;


    public void StartBossUIAnimation()
    {
        _bossCamera.SetActive(true);
        _animator.SetTrigger("ShowTitle");
    }


    public void StartBossFightAnimationEvent()
    {
        _bossCinematicSystem.StartBossFight();
    }

    public void DisableBossCameraAnimationEvent()
    {
        _bossCamera.SetActive(false);
    }

    public void PlayBossIntroSoundAnimationEvent()
    {
        _bossCinematicSystem.PlayBossIntroSound();
    }
}
