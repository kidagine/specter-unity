using TMPro;
using UnityEngine;

public class InventoryMenuButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text = default;
    private Color _defaultTextColor;


    void Start()
    {
        ColorUtility.TryParseHtmlString("#ff175c", out _defaultTextColor);
    }

    public void OnEnterSelect()
    {
        _text.color = Color.black;
    }

    public void OnExitSelect()
    {
        _text.color = _defaultTextColor;
    }

    public void ButtonReset()
    {
        transform.position = new Vector2(transform.position.x - 60, transform.position.y);
        _text.color = Color.black;
    }
}
