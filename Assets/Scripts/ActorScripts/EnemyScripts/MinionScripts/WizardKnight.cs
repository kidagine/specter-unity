using System.Collections;
using UnityEngine;

public class WizardKnight : MonoBehaviour, IDamageable, ITargetObserver, IExpGiver
{
    [SerializeField] private Damager _damager;
    [SerializeField] private GameObject _explosionPrefab = default;
    [SerializeField] private SpriteRenderer _spriteRenderer = default;
    [SerializeField] private Animator _animator = default;
    [SerializeField] private GameObject _shot = default;
    [SerializeField] private Transform _shootPoint = default;
    [SerializeField] private BoxCollider2D _mainCollider = default;
    [SerializeField] private BoxCollider2D _spearCollider = default;
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private Transform _rightCheckGroundPoint = default;
    [SerializeField] private Transform _leftCheckGroundPoint = default;
    [SerializeField] private EntityAudio _wizardKnightAudio = default;
    [SerializeField] private LayerMask _environmentLayerMask = default;
    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private bool _startOnLeft = default;
    [SerializeField] private bool _isUpsideDown = default;
    private readonly int _checkDistanceRay = 1;
    private readonly int _expWorth = 10;
    private Transform _player;
    private int _currentHealth = 1;
    private bool _isLookingRight;
    private bool _isCharging;
    private bool _canAttack = true;
    private bool _checkOnRight = true;
    private bool _checkOnLeft = true;


    void Start()
    {
        if (_startOnLeft)
        {
            InvertMovement();
        }
    }

    void Update()
    {
        CheckGround();
        if (_player != null)
        {
            _shootPoint.up = _player.position - transform.position;
        }

        if (_player != null)
        {
            if (!_isCharging && _canAttack)
            {
                _wizardKnightAudio.Play("WizardKnightCharge");
                Vector2 direction = _player.position - transform.position;
                if (direction.x > 0)
                {
                    _isLookingRight = true;
                    _spriteRenderer.flipX = _isLookingRight;
                }
                else
                {
                    _isLookingRight = false;
                    _spriteRenderer.flipX = _isLookingRight;
                }
                _isCharging = true;
                _moveSpeed = 0;
                _animator.SetBool("HasTarget", true);
            }
        }
    }

    void FixedUpdate()
    {
        Movement();
    }
    

    public void Shoot()
    {
        Instantiate(_shot, _shootPoint.position, _shootPoint.rotation);
        StartCoroutine(CanAttackCoroutine());
        _animator.SetBool("HasTarget", false);
        _isCharging = false;
        if (_isLookingRight)
        {
            _moveSpeed = 2.4f;
        }
        else
        {
            _moveSpeed = -2.4f;
        }
    }

    IEnumerator CanAttackCoroutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(1.5f);
        _canAttack = true;
    }

    public void TakeDamage(int damageAmount, GameObject damagerObject)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0)
        {
            GiveExp();
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _mainCollider.enabled = false;
            _spearCollider.enabled = false;
            _animator.SetTrigger("Die");
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    public void ReceiveTarget(Transform target)
    {
        _player = target;
    }

    public void LostTarget()
    {
        _player = null;
    }

    private void Movement()
    {
        _rigidbody.velocity = new Vector2(_moveSpeed, _rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.gameObject.layer) & _environmentLayerMask) != 0)
        {
            InvertMovement();
        }
    }

    private void CheckGround()
    {
        RaycastHit2D rightHit = Physics2D.Raycast(_rightCheckGroundPoint.position, -_rightCheckGroundPoint.up, _checkDistanceRay, _environmentLayerMask);
        if (rightHit.collider == null)
        {
            if (_checkOnRight)
            {
                _checkOnRight = false;
                InvertMovement();
            }
        }
        if (rightHit.collider != null)
        {
            if (!_checkOnRight)
            {
                _checkOnRight = true;
            }
        }
        RaycastHit2D leftHit = Physics2D.Raycast(_leftCheckGroundPoint.position, -_leftCheckGroundPoint.up, _checkDistanceRay, _environmentLayerMask);
        if (leftHit.collider == null)
        {
            if (_checkOnLeft)
            {
                _checkOnLeft = false;
                InvertMovement();
            }
        }
        if (leftHit.collider != null)
        {
            if (!_checkOnLeft)
            {
                _checkOnLeft = true;
            }
        }
    }

    private void InvertMovement()
    {
        _moveSpeed *= -1;
        if (!_isUpsideDown)
        {
            if (_moveSpeed > 0)
            {
                _isLookingRight = true;
                _spriteRenderer.flipX = _isLookingRight;
            }
            else
            {
                _isLookingRight = false;
                _spriteRenderer.flipX = _isLookingRight;
            }
        }
        else
        {
            if (_moveSpeed < 0)
            {
                _isLookingRight = true;
                _spriteRenderer.flipX = _isLookingRight;
            }
            else
            {
                _isLookingRight = false;
                _spriteRenderer.flipX = _isLookingRight;
            }
        }
    }

    public void GiveExp()
    {
        GameManager.Instance.GivePlayerExp(_expWorth);
    }
}
