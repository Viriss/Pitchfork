public class oUnitSkill
{
    public string Name { get; set; }
    public SkillType SkillType { get; set; }
    //public ColorType ColorType { get; set; }
    public int Cost { get; set; }
    public string Effect { get; set; }

    public oUnitSkill()
    {
        Name = "";
        //ColorType = GFallUnit.ColorType.Red;
        Cost = 1;
        Effect = "";
    }
    public oUnitSkill(string Name, SkillType SkillType, int Cost, string Effect)
        : this()
    {
        this.Name = Name;
        this.SkillType = SkillType;
        this.Cost = Cost;
        this.Effect = Effect;
    }
    //public oUnitSkill(string Name, ColorType Color, int Cost, string Effect)
    //    : this()
    //{
    //    this.Name = Name;
    //    this.ColorType = Color;
    //    this.Cost = Cost;
    //    this.Effect = effect;
    //}
}
