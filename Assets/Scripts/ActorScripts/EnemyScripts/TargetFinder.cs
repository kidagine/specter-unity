using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private GameObject _targetObserverObject = default;
    [SerializeField] private LayerMask _targetLayer = default;
    private ITargetObserver _targetObserver;


    void Start()
    {
        if (_targetObserverObject.TryGetComponent(out ITargetObserver targetObserver))
        {
            _targetObserver = targetObserver;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.gameObject.layer) & _targetLayer) != 0)
        {
            _targetObserver.ReceiveTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.gameObject.layer) & _targetLayer) != 0)
        {
            _targetObserver.LostTarget();
        }
    }
}
