using System.Collections;
using UnityEngine;

public class Charon : MonoBehaviour, IDamageable
{
	[SerializeField] private Transform _playerTransform = default;
	[SerializeField] private Transform _shootPoint = default;
	[SerializeField] private GameObject _charonShot = default;
	[SerializeField] private BossHealthUI _bossHealthUI = default;
	private readonly int _moveSpeed = 3;
	private readonly int _shootTimes = 3;	
	private int _currentHealth = 30;
	private Vector2 _moveToPosition;


	void Start()
    {
		StartCoroutine(ShootPatternCoroutine());
    }

	private void Update()
	{
		transform.position = Vector2.MoveTowards(transform.position, new Vector2(_playerTransform.position.x, transform.position.y), _moveSpeed * Time.deltaTime);
		//Vector2 direction = (_playerTransform.position - transform.position).normalized;
		//_moveToPosition = new Vector2(transform.position.x + direction.x, transform.position.y);
	}

	IEnumerator ShootPatternCoroutine()
	{
		for (int i = 0; i < _shootTimes; i++)
		{
			yield return new WaitForSeconds(0.5f);
			Instantiate(_charonShot, _shootPoint.position, Quaternion.identity);
		}
		yield return new WaitForSeconds(2f);
		StartCoroutine(ShootPatternCoroutine());
	}

	public void TakeDamage(int damageAmount, GameObject damagerObject)
	{
		_currentHealth -= damageAmount;
		_bossHealthUI.SetHealth(_currentHealth);
		if (_currentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}
}
