using System.Collections;
using UnityEngine;

public class TheWall : MonoBehaviour, IDamageable, ITargetObserver
{
	[SerializeField] private GameObject _shot = default;
	[SerializeField] private GameObject _explosionPrefab = default;
	[SerializeField] private Transform _shootPoint = default;
	[SerializeField] private SpriteRenderer _spriteRenderer = default;
	private Transform _playerPosition;
	private Color _normalColor;
	private Color _hurtColor;
	private int _currentHealth = 30;


	void Start()
	{
		ColorUtility.TryParseHtmlString("#ff175c", out _hurtColor);
		ColorUtility.TryParseHtmlString("#ffffff", out _normalColor);
		StartCoroutine(ShootPatternCoroutine());
	}

	void Update()
	{
		if (_playerPosition != null)
		{
			_shootPoint.up = _playerPosition.position - transform.position;
		}
	}

	IEnumerator ShootPatternCoroutine()
	{
		yield return new WaitForSeconds(3.0f);
		if (_playerPosition != null)
		{
			Instantiate(_shot, _shootPoint.position, _shootPoint.rotation);
		}
		StartCoroutine(ShootPatternCoroutine());
	}

	public void TakeDamage(int damageAmount, GameObject damagerObject)
	{
		StartCoroutine(HurtEffect());
		_currentHealth -= damageAmount;
		if (_currentHealth <= 0)
		{
			Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	IEnumerator HurtEffect()
	{
		_spriteRenderer.color = _hurtColor;
		yield return new WaitForSeconds(0.1f);
		_spriteRenderer.color = _normalColor;
	}

	public void ReceiveTarget(Transform target)
	{
		_playerPosition = target;
	}

	public void LostTarget()
	{
		_playerPosition = null;
	}
}
