using UnityEngine;

public class ObstacleDelayer : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private AreaSound _areaSound = default;
    [SerializeField] private float _timeInBetween = 1.0f;


    void Start()
    {
        _animator.enabled = false;
    }

    void Update()
    {
        if (!_animator.isActiveAndEnabled)
        {
            _timeInBetween -= Time.deltaTime;
            if (_timeInBetween <= 0)
            {
                _animator.enabled = true;
            }
        }
    }

    public void PlayAreaSoundAnimationEvent()
    {
        _areaSound.PlayAreaSound();
    }
}
