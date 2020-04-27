using TMPro;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Animator _animator = default;
    [SerializeField] private TextMeshProUGUI _text = default;


    public void OnEnterSelect()
    {
        _animator.SetBool("IsSelected", true);
        _text.color = Color.black;
    }

    public void OnExitSelect()
    {
        _animator.SetBool("IsSelected", false);
        _text.color = Color.white;
    }

    public void ButtonReset()
    {
        transform.position = new Vector2(transform.position.x - 60, transform.position.y);
        _text.color = Color.black;
    }
}
