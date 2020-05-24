using UnityEngine;

public class WizardKnightAnimationEvent : MonoBehaviour
{
    [SerializeField] private WizardKnight _wizardKnight = default;


    public void DieAnimationEvent()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ShootAnimationEvent()
    {
        _wizardKnight.Shoot();
    }
}
