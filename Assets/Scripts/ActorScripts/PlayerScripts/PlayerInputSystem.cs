using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAim))]
public class PlayerInputSystem : MonoBehaviour
{
	[SerializeField] private Player _player = default;
	[SerializeField] private PlayerMovement _playerMovement = default;
	[SerializeField] private PlayerAim _playerAim = default;
	[SerializeField] private PlayerDialogueSystem _playerDialogue = default;
	private PlayerInputActions _playerInputActions;


	private void Awake()
	{
		_playerInputActions = new PlayerInputActions();
		_playerInputActions.PlayerControls.Movement.performed += SetMove;
		_playerInputActions.PlayerControls.Aim.performed += SetAim;
		_playerInputActions.PlayerControls.Jump.performed += Jump;
		_playerInputActions.PlayerControls.Dash.performed += Dash;
		_playerInputActions.PlayerControls.Melee.performed += Melee;
		_playerInputActions.PlayerControls.ChargeShot.performed += ChargeShot;
		_playerInputActions.PlayerControls.ChargeShot.canceled += Shoot;
		_playerInputActions.PlayerControls.ChargeDash.performed += ChargeDash;
		_playerInputActions.PlayerControls.ChargeDash.canceled += Dash;
		_playerInputActions.PlayerControls.Inventory.performed += Inventory;
		_playerInputActions.PlayerControls.Talk.performed += Talk;
	}

	private void SetMove(InputAction.CallbackContext context)
	{
		_playerMovement.MovementInput = context.ReadValue<Vector2>();
	}

	private void SetAim(InputAction.CallbackContext context)
	{
		_playerAim.AimInput = context.ReadValue<Vector2>();
	}

	private void Jump(InputAction.CallbackContext context)
	{
		_playerMovement.Jump();
	}

	private void Melee(InputAction.CallbackContext context)
	{
		_player.Melee();
	}

	private void ChargeShot(InputAction.CallbackContext context)
	{
		_player.ChargeShot();
	}

	private void Shoot(InputAction.CallbackContext context)
	{
		_player.Shoot();
	}
	private void ChargeDash(InputAction.CallbackContext context)
	{
		_playerAim.IsChargingDash = true;
	}

	private void Dash(InputAction.CallbackContext context)
	{
		_playerMovement.Dash();
		_playerAim.IsChargingDash = false;
	}

	private void Inventory(InputAction.CallbackContext context)
	{
		_player.Inventory();
	}

	private void Talk(InputAction.CallbackContext context)
	{
		_playerDialogue.Talk();
	}

	private void OnEnable()
	{
		_playerInputActions.Enable();
	}

	private void OnDisable()
	{
		_playerInputActions.Disable();
	}
}
