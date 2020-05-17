using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InventoryUIAnimationEvent))]
[RequireComponent(typeof(EntityAudio))]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private Camera _camera = default;
    [SerializeField] private GameObject _inventoryObject = default;
    [SerializeField] private Transform _inventoryPlayerPoint = default;
    [SerializeField] private Transform _inventoryPoint = default;
    [SerializeField] private EntityAudio _inventoryAudio = default;
    [SerializeField] private Player _player = default;
	private readonly int _outOfBoundsX = 600;
	private readonly int _outOfBoundsY = 235;
	private bool _isResumedPressed;


    public void SetInventory(bool state)
    {
        _animator.SetBool("IsInventoryOpen", state);
        if (state)
        {
            _inventoryObject.SetActive(state);
            PositionInventory();
        }
        _isResumedPressed = false;
    }

    private void PositionInventory()
    {
        _inventoryPoint.position = _camera.WorldToScreenPoint(new Vector2(_inventoryPlayerPoint.position.x, _inventoryPlayerPoint.position.y));
        if (_inventoryPoint.localPosition.x < -_outOfBoundsX || _inventoryPoint.localPosition.x > _outOfBoundsX
           || _inventoryPoint.localPosition.y < -_outOfBoundsY || _inventoryPoint.localPosition.y > _outOfBoundsY)
        {
            _inventoryPoint.localPosition = Vector2.zero;
        }
    }

    public void Resume()
    {
        if (!_isResumedPressed)
        {
            _inventoryAudio.Play("Click");
            _player.Inventory();
            _isResumedPressed = true;
        }
    }

    public void Exit()
    {
        _inventoryAudio.Play("Click");
        Application.Quit();
    }

    public void Hover()
    {
        _inventoryAudio.Play("Hover");
    }

    public void CloseInventory()
    {
        _inventoryObject.SetActive(false);
    }
}