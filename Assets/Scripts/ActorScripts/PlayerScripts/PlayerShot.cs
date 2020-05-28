using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody = default;
    [SerializeField] private GameObject _playerShotExplosion = default;
    [SerializeField] private LayerMask _attackLayers = default;
	[SerializeField] private int _forceSpeed = 1000;


    void Start()
    {
        _rigidbody.AddForce(transform.up * _forceSpeed);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.gameObject.layer) & _attackLayers) != 0)
        {
            Instantiate(_playerShotExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}