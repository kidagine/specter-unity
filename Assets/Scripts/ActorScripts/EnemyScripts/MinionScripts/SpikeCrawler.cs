using UnityEngine;

public class SpikeCrawler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private Transform _rightCheckGroundPoint = default;
    [SerializeField] private Transform _leftCheckGroundPoint = default;
    [SerializeField] private Transform _horizontalCheckWallPoint = default;
    [SerializeField] private BoxCollider2D _boxCollider = default;
    [SerializeField] private LayerMask _environmentLayerMask = default;
    [SerializeField] private bool _startOnLeft = default;
    private readonly int _checkDistanceRay = 2;
    private bool _checkOnRight = true;
    private bool _checkOnLeft = true;
    private int _moveSpeed = 2;


    void Start()
    {
        if (_startOnLeft)
        {
            InvertMovement();
        }
    }

    void Update()
    {
        CheckCanMove();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rigidbody.velocity = new Vector2(_moveSpeed, _rigidbody.velocity.y);
    }

    private void CheckCanMove()
    {
        CheckWall();
        CheckGround();
    }

    private void CheckWall()
    {
        RaycastHit2D horizontalHit = Physics2D.Raycast(_horizontalCheckWallPoint.position, _horizontalCheckWallPoint.right, _boxCollider.bounds.size.x + 1.0f, _environmentLayerMask);
        if (horizontalHit.collider != null)
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
        //if (_moveSpeed < 0)
        //{
        //    _rightCheckGroundPoint.gameObject.SetActive(false);
        //    _leftCheckGroundPoint.gameObject.SetActive(true);
        //}
        //else
        //{
        //    _rightCheckGroundPoint.gameObject.SetActive(true);
        //    _leftCheckGroundPoint.gameObject.SetActive(false);
        //}
    }
}
