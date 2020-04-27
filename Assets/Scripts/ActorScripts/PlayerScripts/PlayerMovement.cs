using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(EntityAudio))]
[RequireComponent(typeof(PlayerAim))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private BoxCollider2D _boxCollider = default;
    [SerializeField] private BoxCollider2D _dashCollider = default;
    [SerializeField] private SpriteRenderer _spriteRenderer = default;
    [SerializeField] private Transform _groundCheckPoint = default;
    [SerializeField] private Transform _meleePoint = default;
    [SerializeField] private Transform _inventoryPoint = default;
    [SerializeField] private GameObject _dashEffectPrefab = default;
    [SerializeField] private GameObject _dashLandEffectPrefab = default;
    [SerializeField] private Player _player = default;
    [SerializeField] private EntityAudio _playerAudio = default;
    [SerializeField] private PlayerAim _playerAim = default;
    [SerializeField] private LayerMask _environmentLayerMask = default;
    private readonly float _maximumFootstepCooldown = 0.3f;
    private readonly float _maximumJumpCooldown = 0.15f;
    private readonly int _moveSpeed = 5;
    private readonly int _jumpImpulse = 10;
    private readonly float _dashImpulse = 5.5f;
    private readonly int _knockbackForce = 6;
    private float _currentFootstepCooldown;
    private float _currentJumpCooldown = 0.15f;
    private bool _isMovementLocked;
    private bool _isRotationLocked;
    private bool _isSpriteFlipped;

    public bool IsGrounded { get; private set; }
    public bool IsDashing { get; set; }
    public Vector2 MovementInput { private get; set; }


    void Update()
    {
        CheckGrounded();
        HandleSpriteFlip();
        Footsteps();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (!_isMovementLocked && !IsDashing)
        {
            _rigidbody.velocity = new Vector2(MovementInput.x * _moveSpeed, _rigidbody.velocity.y);
            _animator.SetFloat("Movement", Mathf.Abs(MovementInput.x));
        }
    }

    public void CheckGrounded()
    {
        Vector2 boxSize = new Vector2(_boxCollider.bounds.size.x, 0.15f);
        if (Physics2D.OverlapBox(_groundCheckPoint.position, boxSize, 0.0f, _environmentLayerMask))
        {
            if (!IsGrounded)
            {
                IsDashing = false;
                _playerAudio.Play("PlayerLand");
            }
            IsGrounded = true;
            _animator.SetBool("IsGrounded", true);
        }
        else
        {
            _currentJumpCooldown -= Time.deltaTime;
            if (_currentJumpCooldown <= 0)
            {
                IsGrounded = false;
                _animator.SetBool("IsGrounded", false);
                _currentJumpCooldown = _maximumJumpCooldown;
            }
        }
    }

    private void HandleSpriteFlip()
    {
        if (!_isRotationLocked)
        {
            if (!_player.IsAttacking && MovementInput.x != 0.0f)
            {
                MovementSpriteFlip();
            }
            else
            {
                AimingSpriteFlip();
            }
        }
    }

    private void MovementSpriteFlip()
    {
        if (MovementInput.x > 0.0f)
        {
            _inventoryPoint.localPosition = new Vector2(5.5f, _inventoryPoint.localPosition.y);
            _meleePoint.localPosition = new Vector2(1.5f, _meleePoint.localPosition.y);
            _meleePoint.rotation = Quaternion.identity;
            _isSpriteFlipped = false;
            _spriteRenderer.flipX = _isSpriteFlipped;

        }
        else if (MovementInput.x < 0.0f)
        {
            _inventoryPoint.localPosition = new Vector2(-5.5f, _inventoryPoint.localPosition.y);
            _meleePoint.localPosition = new Vector2(-1.5f, _meleePoint.localPosition.y);
            _meleePoint.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            _isSpriteFlipped = true;
            _spriteRenderer.flipX = _isSpriteFlipped;
        }
        else
        {
            _spriteRenderer.flipX = _isSpriteFlipped;
        }
    }

    private void AimingSpriteFlip()
    {
        if (_playerAim.GetAimingDirection() >= 0.0f)
        {
            if (_spriteRenderer.flipX)
            {
                _playerAudio.Play("PlayerTurnAround");
            }
            _inventoryPoint.localPosition = new Vector2(5.5f, _inventoryPoint.localPosition.y);
            _meleePoint.localPosition = new Vector2(1.5f, _meleePoint.localPosition.y);
            _meleePoint.rotation = Quaternion.identity;
            _isSpriteFlipped = false;
            _spriteRenderer.flipX = _isSpriteFlipped;
        }
        else
        {
            if (!_spriteRenderer.flipX)
            {
                _playerAudio.Play("PlayerTurnAround");
            }
            _inventoryPoint.localPosition = new Vector2(-5.5f, _inventoryPoint.localPosition.y);
            _meleePoint.localPosition = new Vector2(-1.5f, _meleePoint.localPosition.y);
            _meleePoint.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            _isSpriteFlipped = true;
            _spriteRenderer.flipX = _isSpriteFlipped;
        }
    }

    public void Jump()
    {
        if (IsGrounded && !_playerAim.IsChargingDash && !_player.IsAttacking)
        {
            _playerAudio.Play("PlayerJump");
            _rigidbody.AddForce(new Vector2(0.0f, _jumpImpulse), ForceMode2D.Impulse);
            _animator.SetBool("IsGrounded", false);
        }
    }

    public void Dash()
    {
        Vector3 dashToPoint = _playerAim.GetDashToObject();
        if (dashToPoint != Vector3.zero)
        {
            _animator.SetBool("IsDashing", true);
            _playerAudio.Play("PlayerDash");
            _boxCollider.enabled = false;
            _dashCollider.enabled = true;
            IsDashing = true;
            _rigidbody.AddForce((dashToPoint - transform.position) * _dashImpulse ,ForceMode2D.Impulse);
            Instantiate(_dashEffectPrefab, transform.position, transform.rotation);
            _playerAim.SetDashToObjectToNull();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[0].normal;
        AttachToSurface(contactPoint);
    }

    private void AttachToSurface(Vector2 contactPoint)
    {
        if (IsDashing)
        {
            if (contactPoint == new Vector2(0.0f, 1.0f))
            {
                LockMovement(false);
                _rigidbody.gravityScale = 3.0f;
            }
            else
            {
                _animator.SetBool("IsGrounded", true);
                LockMovement(true);
                _rigidbody.gravityScale = 0.0f;
            }
            transform.up = contactPoint;
            ResetPlayerMovement();
            Instantiate(_dashLandEffectPrefab, transform.position, transform.rotation);
        }
    }

    public void ResetPlayerMovement()
    {
        _animator.SetBool("IsDashing", false);
        _animator.SetBool("IsDashLocked", false);
        _boxCollider.enabled = true;
        _dashCollider.enabled = false;
        IsDashing = false;
    }

    public void KnockBack(GameObject damagerObject)
    {
        LockMovement(true);
        Vector2 knockbackDirection = (transform.position - damagerObject.transform.position).normalized;
        _rigidbody.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
        _rigidbody.AddForce(transform.up * _knockbackForce * 1.5f, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocity());
    }

    IEnumerator ResetVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        LockMovement(false);
        _rigidbody.velocity = Vector2.zero;
    }

    private void Footsteps()
    {
        if (IsGrounded && !_isMovementLocked && MovementInput.magnitude > 0.0f)
        {
            _currentFootstepCooldown -= Time.deltaTime;
            if (_currentFootstepCooldown <= 0)
            {
                _playerAudio.PlayRandomFromSoundGroup("PlayerFootsteps");
                _currentFootstepCooldown = _maximumFootstepCooldown;
            }
        }
    }

    public void LockMovement(bool state)
    {
        if (state)
        {
            _isMovementLocked = true;
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _isMovementLocked = false;
        }
    }

    public void LockRotation(bool state)
    {
        if (state)
        {
            _isRotationLocked = true;
        }
        else
        {
            _isRotationLocked = false;
        }
    }
}
