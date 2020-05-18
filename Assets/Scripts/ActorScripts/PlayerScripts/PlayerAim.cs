using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(EntityAudio))]
public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    [SerializeField] private Animator _animator = default;
    [SerializeField] private Transform _firePoint = default;
    [SerializeField] private Transform _firePointPivot = default;
    [SerializeField] private Transform _aimArrowPivot = default;
    [SerializeField] private LineRenderer _lineRenderer = default;
    [SerializeField] private PlayerMovement _playerMovement = default;
    [SerializeField] private Player _player = default;
    [SerializeField] private EntityAudio _playerAudio = default;    
    private readonly float _aimRayDistance = 7.0f;
    private Vector2 _dashToPoint;
    private Color _activeAimColor;
    private Color _disabledAimColor;
    private float _lookDot;
    private bool _isDashLocked;

    public Vector2 AimInput { private get; set; }
    public bool IsChargingDash { get; set; }


    void Start()
    {
        _lineRenderer.useWorldSpace = true;
        ColorUtility.TryParseHtmlString("#ff175c", out _activeAimColor);
        ColorUtility.TryParseHtmlString("#ffffff", out _disabledAimColor);
    }

    void Update()
    {
        AimAtCursor();
		if (GlobalSettings._hasDash)
		{
			HandleDash();
		}
	}

    private void AimAtCursor()
    {
        if (InputManager.Instance.IsGamepadSchemeActive)
        {
            Vector2 aimDirection = AimInput;
            float rangeDot = Vector2.Dot(aimDirection, transform.up);
            _lookDot = Vector2.Dot(aimDirection, transform.right);
            if (IsChargingDash)
            {
                if (rangeDot > 0)
                {
                    _firePointPivot.up = aimDirection;
                }
            }
            else
            {
                _firePointPivot.up = aimDirection;
            }
        }
        else
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = _camera.ScreenToWorldPoint(mousePosition);
            Vector2 aimDirection = new Vector2(mousePosition.x - transform.localPosition.x, mousePosition.y - transform.localPosition.y);
            float rangeDot = Vector2.Dot(aimDirection, transform.up);
            _lookDot = Vector2.Dot(aimDirection, transform.right);
            if (IsChargingDash)
            {
                if (rangeDot > 0)
                {
                    _firePointPivot.up = aimDirection;
                }
            }
            else
            {
                _firePointPivot.up = aimDirection;
            }
        }
    }

    public float GetAimingDirection()
    {
        return _lookDot;
    }

    private void HandleDash()
    {
        if (IsChargingDash && !_player.IsAttacking && _playerMovement.IsGrounded)
        {
            if (!_lineRenderer.enabled)
            {
                _animator.SetBool("IsChargingDash", true);
                _lineRenderer.enabled = true;
                _playerMovement.LockMovement(true);
            }
            ChargeDash();
        }
        else
        {
            if (_lineRenderer.enabled)
            {
                _animator.SetBool("IsChargingDash", false);
                _lineRenderer.enabled = false;
                _playerMovement.LockMovement(false);
            }
        }
    }

    public void ChargeDash()
    {
        _lineRenderer.SetPosition(0, _firePoint.transform.position);
        Ray2D ray = new Ray2D(_firePoint.transform.position, _firePoint.transform.up);
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.transform.position, _firePoint.transform.up, _aimRayDistance);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Dashable"))
            {
                if (!_isDashLocked)
                {
                    _isDashLocked = true;
                    _animator.SetBool("IsDashLocked", true);
                    _playerAudio.Play("PlayerDashLocked");
                    _aimArrowPivot.gameObject.SetActive(true);
                }
                _lineRenderer.SetPosition(1, hit.point);
                _lineRenderer.material.color = _activeAimColor;

                _dashToPoint = hit.point;
                _aimArrowPivot.position = hit.point;
                _aimArrowPivot.up = hit.normal;
            }
            else
            {
                if (_isDashLocked)
                {
                    _isDashLocked = false;
                    _animator.SetBool("IsDashLocked", false);
                    _animator.SetBool("IsDashLocked", false);
                    _aimArrowPivot.gameObject.SetActive(false);
                    _dashToPoint = Vector2.zero;
                }
                _lineRenderer.SetPosition(1, hit.point);
                _lineRenderer.material.color = _disabledAimColor;
            }

        }
        else
        {
            if (_isDashLocked)
            {
                _isDashLocked = false;
                _animator.SetBool("IsDashLocked", false);
                _animator.SetBool("IsDashLocked", false);
                _aimArrowPivot.gameObject.SetActive(false);
                _dashToPoint = Vector2.zero;
            }
            _lineRenderer.SetPosition(1, ray.GetPoint(_aimRayDistance));
            _lineRenderer.material.color = _disabledAimColor;
        }
    }

    public Vector2 GetDashToObject()
    {
        _aimArrowPivot.gameObject.SetActive(false);
        return _dashToPoint;
    }

    public void SetDashToObjectToNull()
    {
        _isDashLocked = false;
        _dashToPoint = Vector2.zero;
    }
}