using UnityEngine;

[RequireComponent(typeof(InventoryUI))]
public class InventoryUIAnimationEvent : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventoryUI = default;


    public void CloseInventoryAnimationEvent()
    {
        _inventoryUI.CloseInventory();
    }
}
