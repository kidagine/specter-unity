using UnityEngine;

[RequireComponent(typeof(EntityAudio))]
public class PlayerShotExplosion : MonoBehaviour
{
    [SerializeField] private EntityAudio _playerShotAudio = default;

    private void Start()
    {
        _playerShotAudio.Play("PlayerShotExplosion");
    }
}
