using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private GameObject _targetObserverObject = default;
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
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _targetObserver.ReceiveTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            Debug.Log("Lost");
            _targetObserver.LostTarget();
        }
    }
}
