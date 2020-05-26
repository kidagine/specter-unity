using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    [SerializeField] private GameObject _cameraToEnable = default;
    [SerializeField] private GameObject _cameraToDisable = default;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _cameraToEnable.SetActive(true);
            _cameraToDisable.SetActive(false);
        }
    }
}
