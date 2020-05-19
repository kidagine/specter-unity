using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private GameObject _playerShotExplosion = default;
	[SerializeField] private int _forceSpeed = 1000;


    void Start()
    {
        _rigidbody.AddForce(transform.up * _forceSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Player player))
        {
            Instantiate(_playerShotExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}