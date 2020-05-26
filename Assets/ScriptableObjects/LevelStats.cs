using UnityEngine;

[CreateAssetMenu(fileName = "LevelStats", menuName = "ScriptableObjects/LevelStats")]
public class LevelStats : ScriptableObject
{
    public int MaxLevel;
    public PlayerLevel[] PlayerLevel;
}
