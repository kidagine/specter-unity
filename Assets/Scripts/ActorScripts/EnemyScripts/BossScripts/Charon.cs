using System.Collections;
using UnityEngine;

public class Charon : MonoBehaviour, IDamageable
{
	[SerializeField] private Transform _playerTransform = default;
	[SerializeField] private Transform _shootPoint = default;
	[SerializeField] private Transform _cameraConfiner = default;
	[SerializeField] private Animator _wallAnimator = default;
	[SerializeField] private SpriteRenderer _spriteRenderer = default;
	[SerializeField] private GameObject _charonShot = default;
	[SerializeField] private GameObject _bossExplosionPrefab = default;
	[SerializeField] private Rigidbody2D _rigidbody = default;
	[SerializeField] private Animator _animator = default;
	[SerializeField] private BossUI _bossUI = default;
	private readonly float _moveAcceleration = 3.0f;
	private Color _normalColor;
	private Color _hurtColor;
	private float _maxSpeed = 4.5f;
	private float _moveSpeed;
	private int _shootTimes = 3;
	private int _currentHealth = 40;


	void Start()
    {
		ColorUtility.TryParseHtmlString("#ff175c", out _hurtColor);
		ColorUtility.TryParseHtmlString("#ffffff", out _normalColor);
		StartCoroutine(ShootPatternCoroutine());
    }

	void Update()
	{
		MoveTowardsPlayer();
	}

	void FixedUpdate()
	{
		Movement();
	}

	private void MoveTowardsPlayer()
	{
		float direction = transform.position.x - _playerTransform.transform.position.x;
		if (direction < 0)
		{
			if (_moveSpeed < _maxSpeed)
			{
				_moveSpeed += Time.deltaTime * _moveAcceleration;
			}
		}
		else if (direction > 0)
		{
			if (_moveSpeed > -_maxSpeed)
			{
				_moveSpeed -= Time.deltaTime * _moveAcceleration;
			}
		}
	}

	private void Movement()
	{
		_rigidbody.velocity = new Vector2(_moveSpeed, _rigidbody.velocity.y);
	}

	IEnumerator ShootPatternCoroutine()
	{
		for (int i = 0; i < _shootTimes; i++)
		{
			yield return new WaitForSeconds(0.5f);
			Instantiate(_charonShot, _shootPoint.position, Quaternion.identity);
		}
		yield return new WaitForSeconds(1.5f);
		StartCoroutine(ShootPatternCoroutine());
	}

	public void TakeDamage(int damageAmount, GameObject damagerObject)
	{
		StartCoroutine(HurtEffect());
		_currentHealth -= damageAmount;
		_bossUI.BossHealthUI.SetHealth(_currentHealth);
		if (_currentHealth <= 20)
		{
			_maxSpeed = 6.0f;
			_shootTimes = 5;
		}
		if (_currentHealth <= 0)
		{
			_cameraConfiner.position = new Vector2(3.0f, _cameraConfiner.position.y);
			_cameraConfiner.localScale = new Vector2(48.0f, _cameraConfiner.localScale.y);
			_wallAnimator.SetTrigger("Open");
			_bossUI.BossVanishedUI.ShowVanished();
			_bossUI.BossHealthUI.ShowHealth(false);
			Instantiate(_bossExplosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	IEnumerator HurtEffect()
	{
		_spriteRenderer.color = _hurtColor;
		yield return new WaitForSeconds(0.1f);
		_spriteRenderer.color = _normalColor;
	}
}
