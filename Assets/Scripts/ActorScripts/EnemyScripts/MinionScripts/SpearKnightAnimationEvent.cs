using UnityEngine;

public class SpearKnightAnimationEvent : MonoBehaviour
{
    [SerializeField] private SpearKnight _spearKnight = default;


    public void DieAnimationEvent()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ChargeToTargetAnimationEvent()
    {
        _spearKnight.ChargeToTarget();
    }
}
