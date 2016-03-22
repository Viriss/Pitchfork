using System;

public static class Globals
{
    public static int MaxLevel = 40;
    public static int ExpRate = 25;

    public static int LevelFromExp(int Exp)
    {
        return (int)(Math.Sqrt(ExpRate * ExpRate + (ExpRate * 4) * Exp) - ExpRate) / (ExpRate * 2);
    }
    public static int MinExpForLevel(int Level)
    {
        return ExpRate * Level * (1 + Level);
    }

    public static float Clamp(float value, float min, float max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }
}

public enum CardImage
{
    blank,
    battlehunk,
    cleric,
    darklancer,
    swordsman
}

public enum ColorIdentity
{
    Sword = 0,
    Red = 1, 
    Blue = 2, 
    Green = 4, 
    Yellow = 8, 
    Purple = 16, 
    Brown = 32,
 
    RedBlue = Red + Blue,
    RedGreen = Red + Green,
    RedYellow = Red + Yellow,
    RedPurple = Red + Purple,
    RedBrown = Red + Brown,

    BlueGreen = Blue + Green,
    BlueYellow = Blue + Yellow,
    BluePurple = Blue + Purple,
    BlueBrown = Blue + Brown,

    GreenYellow = Green + Yellow,
    GreenPurple = Green + Purple,
    GreenBrown = Green + Brown,

    YellowPurple = Yellow + Purple,
    YellowBrown = Yellow + Brown,

    PurpleBrown = Purple + Brown
}
public enum SkillType { Combat, GemBoard, Passive }
public enum TeamType { Good, Bad, Other }
public enum UnitStat { Attack, Defense, Health, Magic }

