using System;
using System.Collections.Generic;

public class oUnit
{
    public string Name { get; set; }
    public Guid ID = Guid.NewGuid();
    public CardImage CardImage { get; set; }
    public ColorIdentity ColorIdentity { get; set; }
    public int Att { get { return Lerp.Stat(_minAtt, _maxAtt, Level) + _modAtt; } }
    public int Def { get { return Lerp.Stat(_minDef, _maxDef, Level) + _modDef; } }
    public int MaxHP { get { return Lerp.Stat(_minHP, _maxHP, Level) + _modHP; } }
    public int MaxMagic { get { return Lerp.Stat(_minMagic, _maxMagic, Level) + _modMagic; } }
    public int CurHP { get; set; }

    public int CurAtt;
    public int CurDef;
    public int CurMagic;
    
    private int _curMana;
    public int CurMana { get { return _curMana; } }
   
    public int Exp { get; set; }
    public int Level { get { return Globals.LevelFromExp(Exp); } }
    public List<oUnitSkill> Skills;
    public List<oStatMod> Modifiers;
    public bool isActive;
    public bool isSilenced;
    public bool isBurning;
    public bool isSleeping;
    public bool isParalyzed;
    public bool isFrozen;
    public bool isWebbed;

    public float ManaRatio { get { return (float)CurMana / (float)Skills[0].Cost; } }

    private int _minAtt;
    private int _maxAtt;
    private int _modAtt;
    private int _minDef;
    private int _maxDef;
    private int _modDef;
    private int _minHP;
    private int _maxHP;
    private int _modHP;
    private int _minMagic;
    private int _maxMagic;
    private int _modMagic;

    public oUnit()
    {
        Exp = 0;
        Name = "";
        CardImage = CardImage.blank;
        Skills = new List<oUnitSkill>();
        Modifiers = new List<oStatMod>();
        CurHP = 0;
        _curMana = 0;
        isActive = true;
        isSilenced = false;
        isBurning = false;
        isSleeping = false;
        isParalyzed = false;
        isFrozen = false;
        isWebbed = false;

        _minAtt = 0;
        _maxAtt = 0;
        _modAtt = 0;
        _minDef = 0;
        _maxDef = 0;
        _modDef = 0;
        _minHP = 0;
        _maxHP = 0;
        _modHP = 0;
        _minMagic = 0;
        _maxMagic = 0;
        _modMagic = 0;
    }
    public oUnit(string Name, oRange Att, oRange Def, oRange HP, oRange Magic)
        : this()
    {
        this.Name = Name;

        _minAtt = Att.Min;
        _maxAtt = Att.Max;

        _minDef = Def.Min;
        _maxDef = Def.Max;

        _minMagic = Magic.Min;
        _maxMagic = Magic.Max;

        _minHP = HP.Min;
        _maxHP = HP.Max;
        CurHP = MaxHP;

        ResetValues();
    }

    public int AddMana(ColorIdentity Color, int Amount)
    {
        foreach (oUnitSkill sk in Skills)
        {
            //Skill colors if goes here!!!! 2/27/2016

            if (CurMana + Amount > sk.Cost)
            {
                Amount -= sk.Cost - CurMana;
                _curMana = sk.Cost;
            }
            else
            {
                _curMana += Amount;
                return 0;
            }

            if (Amount == 0) { break; }
        }
        return Amount;
    }
    public void AddSkill(oUnitSkill Skill)
    {
        Skills.Add(Skill);
    }
    public void EmptyMana()
    {
        _curMana = 0;
    }
    public void ReceiveDamage(int Damage) { ReceiveDamage(Damage, false, false); }
    public void ReceiveDamage(int Damage, bool onlyHealth, bool OnlyArmor)
    {
        if (onlyHealth)
        {
            CurHP -= Damage;
        }
        else if (OnlyArmor)
        {
            CurDef -= Damage;
        }
        else
        {
            CurDef -= Damage;
            if (CurDef < 0)
            {
                CurHP += CurDef;
            }
        }
        if (CurHP < 0) { CurHP = 0; }
        if (CurDef < 0) { CurDef = 0; }
    }
    public void ResetValues()
    {
        CurAtt = Att;
        CurDef = Def;
        CurHP = MaxHP;
        CurMagic = MaxMagic;
    }
    public float PowerPerc()
    {
        int max = 0;
        foreach (oUnitSkill s in Skills)
        {
            if (s.Cost > max) { max = s.Cost; }
        }
        return Globals.Clamp((float)CurMana / (float)max, 0.0f, 1.0f);
    }
}
