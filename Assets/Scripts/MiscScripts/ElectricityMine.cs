using UnityEngine;

public class ElectricityMine : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
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
}
