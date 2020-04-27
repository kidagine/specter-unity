using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private Player _player = default;
    [SerializeField] private PlayerUI _playerUI = default;


    public void CreateShotAnimationEvent()
    {
        _player.HasCharged();
    }

    public void OpenInventoryAnimationEvent()
    {
        _playerUI.InventoryUI.SetInventory(true);
    }

    public void CloseInventoryAnimationEvent()
    {
        _playerUI.InventoryUI.SetInventory(false);
    }
}
