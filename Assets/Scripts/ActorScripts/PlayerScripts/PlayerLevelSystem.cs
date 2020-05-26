using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    [SerializeField] private LevelStats _levelStats = default;
    [SerializeField] private PlayerUI _playerUI = default;
    [SerializeField] private Player _player = default;
    private int _expCap;
    private int _currentExp;
    private int _currentLevel;
    private int _levelIndex;
    private int _reservedExp;


    void Start()
    {
        _levelIndex = GlobalSettings._playerLevel;
        _currentExp = GlobalSettings._playerExp;
        SetLevel(false);
    }

    public void EarnExp(int earnedExp)
    {
        if (_currentLevel < _levelStats.MaxLevel)
        {
            _currentExp += earnedExp;
            if (_currentExp < _expCap)
            {
                GlobalSettings._playerExp = _currentExp;
                _playerUI.StatsUI.SetExp(_currentExp);
            }
            else
            {
                _reservedExp = _currentExp - _expCap;
                SetLevel(true);
            }
        }
    }

    private void SetLevel(bool isNotInitial)
    {
        GlobalSettings._playerLevel = _levelIndex;
        PlayerLevel playerLevel = _levelStats.PlayerLevel[_levelIndex];
        _expCap = playerLevel.LevelCap;
        _currentLevel = playerLevel.Level;
        _playerUI.StatsUI.LevelUp(_currentLevel, _expCap, isNotInitial, playerLevel.perk);

        if (_currentLevel < _levelStats.MaxLevel)
        {
            if (isNotInitial)
            {
                _currentExp = _reservedExp;
                GlobalSettings._playerExp = _currentExp;
            }
        }
        else
        {
            _currentExp = 0;
        }
        _playerUI.StatsUI.SetExp(_currentExp);

        _player.SetHealth(playerLevel.Hearts, isNotInitial);
        _player.SetAttack(playerLevel.Attack);
        _levelIndex++;
    }
}
