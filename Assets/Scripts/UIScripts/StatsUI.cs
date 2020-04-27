using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Image[] _heartsImage = default;
    [SerializeField] private Sprite _fullHeartSprite = default;
    [SerializeField] private Sprite _emptyHeartSprite = default;
    [SerializeField] private Slider _expSlider = default;


    public void SetHearts(int currentHearts)
    {
        for (int i = 0; i < _heartsImage.Length; i++)
        {
            _heartsImage[i].sprite = _emptyHeartSprite;
        }

        for (int i = 0; i < currentHearts; i++)
        {
            int heartIndex = i;
            _heartsImage[heartIndex].sprite = _fullHeartSprite;
        }
    }

    public void SetExp(int currentExp)
    {
        _expSlider.value = currentExp;
    }
}
