
using System;

public enum Perk { Health, Attack };

[Serializable]
public struct PlayerLevel
{
    public int Level;
    public int LevelCap;
    public int Hearts;
    public int Attack;
    public Perk perk;
}
