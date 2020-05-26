using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private EntityAudio _areaSound = default;
    private bool _isPlayerNear;


    public void PlayAreaSound()
    {
        if (_isPlayerNear)
        {
            _areaSound.Play("AreaSound");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _isPlayerNear = false;
        }
    }
}
