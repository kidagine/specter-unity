using UnityEngine;

public class SpikeCrawler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private SpriteRenderer _spriteRenderer = default;
    [SerializeField] private Transform _rightCheckGroundPoint = default;
    [SerializeField] private Transform _leftCheckGroundPoint = default;
    [SerializeField] private LayerMask _environmentLayerMask = default;
    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private bool _startOnLeft = default;
    private readonly int _checkDistanceRay = 2;
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
    }

    void FixedUpdate()
    {
        Movement();
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
        RaycastHit2D rightHit = Physics2D.Raycast(_rightCheckGroundPoint.position, -_rightCheckGroundPoint.up, _checkDistanceRay);
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
        RaycastHit2D leftHit = Physics2D.Raycast(_leftCheckGroundPoint.position, -_leftCheckGroundPoint.up, _checkDistanceRay);
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
        if (_moveSpeed > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}
