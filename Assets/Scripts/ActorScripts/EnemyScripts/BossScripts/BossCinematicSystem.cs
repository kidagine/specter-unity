using UnityEngine;

public class BossCinematicSystem : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private BossUI _bossUI = default;
    [SerializeField] private Charon _charon = default;
    [SerializeField] private PlayerCinematic _playerCinematicSystem = default;


    void Awake()
    {
        _playerCinematicSystem.ExitDoorEvent += StartBossCinematic;
        _animator.enabled = false;
        _charon.enabled = false;        
    }

    public void StartBossCinematic()
    {
        _playerCinematicSystem.ExitDoorEvent -= StartBossCinematic;
        _playerCinematicSystem.PlayerCinematicPause(true);
        _bossUI.BossTitleUI.StartBossUIAnimation();
    }

    public void StartBossFight()
    {
        _bossUI.BossHealthUI.ShowHealth(true);
        _playerCinematicSystem.PlayerCinematicPause(false);
        _animator.enabled = true;
        _charon.enabled = true;
    }
}
