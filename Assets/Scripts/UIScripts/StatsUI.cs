using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Image[] _heartsImage = default;
    [SerializeField] private Sprite _fullHeartSprite = default;
    [SerializeField] private Sprite _emptyHeartSprite = default;
    [SerializeField] private Slider _expSlider = default;
    [SerializeField] private TextMeshProUGUI _levelText = default;
    [SerializeField] private TextMeshProUGUI _perkText = default;
    [SerializeField] private Animator _levelUpAnimator = default;
    [SerializeField] private EntityAudio _playerUIAudio = default;


    public void SetHearts(int maxHearts, int currentHearts)
    {
        for (int i = 0; i < _heartsImage.Length; i++)
        {
            if (i < maxHearts)
            {
                _heartsImage[i].enabled = true;
                _heartsImage[i].sprite = _emptyHeartSprite;
            }
            else
            {
                _heartsImage[i].enabled = false;
            }
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

    public void LevelUp(int level, int expCap, bool showLevelUp, Perk perk)
    {
        _expSlider.maxValue = expCap;
        _levelText.text = "-LV" + level.ToString() + "-";
        if (showLevelUp)
        {
            switch (perk)
            {
                case Perk.Health:
                    _perkText.text = "Increased Health";
                    break;
                case Perk.Attack:
                    _perkText.text = "Increased Attack";
                    break;
            }
            _playerUIAudio.Play("PlayerLevelUp");
            _levelUpAnimator.SetTrigger("Show");
        }
    }
}
