using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject _gate = default;
    [SerializeField] private BoxCollider2D _boxCollider = default;
    [SerializeField] private SpriteRenderer _spriteRenderer = default;
    [SerializeField] private Sprite _openSprite = default;
    [SerializeField] private EntityAudio _entityAudio = default;
    private readonly float _yPromptOffset = 1.3f;


    public void OpenGate()
    {
        _entityAudio.Play("LeverOpen");
        _spriteRenderer.sprite = _openSprite;
        _boxCollider.enabled = false;
        Destroy(_gate);
    }

    public Vector2 GetPromptPosition()
    {
        Vector2 promptPosition = new Vector2(transform.position.x, transform.position.y + _yPromptOffset);
        return promptPosition;
    }

}
