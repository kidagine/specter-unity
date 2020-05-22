using UnityEngine;

public interface ITargetObserver
{
    void ReceiveTarget(Transform target);
    void LostTarget();
}
