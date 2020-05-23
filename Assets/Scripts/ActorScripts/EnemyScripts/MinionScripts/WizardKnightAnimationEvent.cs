using UnityEngine;

public class WizardKnightAnimationEvent : MonoBehaviour
{
    [SerializeField] private WizardKnight _wizardKnight = default;


    public void DieAnimationEvent()
    {
        Destroy(gameObject);
    }

    public void ShootAnimationEvent()
    {
        _wizardKnight.Shoot();
    }
}
