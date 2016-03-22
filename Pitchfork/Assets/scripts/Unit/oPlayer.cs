using System;
using System.Collections;
using System.Collections.Generic;

public class oPlayer
{
    public string Name;
    public List<oUnit> Team;
    public TeamType TeamType;

    public oPlayer()
    {
        Name = "";
        Team = new List<oUnit>();
        TeamType = TeamType.Other;
    }

    public void AddEnergy(Hashtable Score)
    {
        ColorIdentity colorID = ColorIdentity.Sword;

        foreach (ColorType color in Score.Keys)
        {
            switch(color)
            {
                case ColorType.Blue:
                    colorID = ColorIdentity.Blue;
                    break;
                case ColorType.Brown:
                    colorID = ColorIdentity.Brown;
                    break;
                case ColorType.Green:
                    colorID = ColorIdentity.Green;
                    break;
                case ColorType.Purple:
                    colorID = ColorIdentity.Purple;
                    break;
                case ColorType.Red:
                    colorID = ColorIdentity.Red;
                    break;
                case ColorType.Yellow:
                    colorID = ColorIdentity.Yellow;
                    break;
            }

            if (colorID != ColorIdentity.Sword)
            {
                AddEnergy(colorID, (int)Score[color]);
            }
        }
    }
    public void AddEnergy(ColorIdentity Color, int Amount)
    {
        foreach(oUnit u in Team)
        {
            if (u.CurHP > 0)
            {
                if (u.isActive && (u.ColorIdentity & Color) == Color && !u.isSilenced)
                {
                    Amount = u.AddMana(Color, Amount); // if over max, give to next unit?  What is max?
                }
            }
            if (Amount == 0) { break; }
        }
    }
    public bool AnyUnitReadyToFire()
    {
        foreach(oUnit u in Team)
        {
            if (u.ManaRatio == 1) { return true; }
        }
        return false;
    }
    public void ReceiveDamage(Guid CardID, int Damage)
    {
        foreach(oUnit unit in Team)
        {
            if (unit.ID == CardID)
            {
                unit.ReceiveDamage(Damage);
            }
        }
    }
    public oUnit GetFirstActiveUnit()
    {
        foreach(oUnit unit in Team)
        {
            if (unit.CurHP > 0) { return unit; }
        }
        return null;
    }
}
