using UnityEngine;

public class CinematicHandler : MonoBehaviour
{
    [SerializeField] private GameObject _cinematicObject = default;
    [SerializeField] private BoxCollider2D _npcBoxCollider = default;
    [SerializeField] private PlayerCinematic _playerCinematic = default;
    [SerializeField] private GameObject _playerPoints = default;


    void Awake()
    {
        if (GlobalSettings._hasDash)
        {
            _cinematicObject.SetActive(false);
            _npcBoxCollider.enabled = false;
            _playerCinematic.StartPlayerCinematicIntro();
        }
    }
}
