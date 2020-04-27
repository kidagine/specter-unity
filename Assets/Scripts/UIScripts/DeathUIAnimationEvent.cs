using UnityEngine;

public class DeathUIAnimationEvent : MonoBehaviour
{
    [SerializeField] private DeathUI _deathUI = default;


    public void RestartAnimationEvent()
    {
        _deathUI.Restart();
    }
}
