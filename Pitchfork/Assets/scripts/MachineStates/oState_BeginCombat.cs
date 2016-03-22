using UnityEngine;
using System.Collections;

public class oState_BeginCombat : oStateMachine
{
    private GlobalCombat _gm;

    public override void Entry()
    {
        _gm = GlobalCombat.GM;

        //_startUpdate = true;
        LoadTeams();

        StartCoroutine(LoadingWait());
    }
    public override void Active()
    {

    }
    void Update()
    {
    }
    public override void Exit()
    {
        GlobalCombat.GM.AddState(typeof(oStateTop));
    }


    IEnumerator LoadingWait()
    {
        PopulateScreen();

        yield return new WaitForSeconds(1);

        _gm.SetTravelTime(_gm.TravelTime);

        yield return new WaitForSeconds(_gm.TravelTime);

        _gm.LockMoving();
        _gm.UpdateGemPositions();

        State = MachineState.Exit;
    }

    public void PopulateScreen()
    {
        foreach (oGridPoint p in GlobalCombat.GM.Grid.Points)
        {
            GameObject gem = (GameObject)Instantiate(_gm.Gem); // Transform.Instantiate(Gem);
            GemLogic gl = gem.GetComponent<GemLogic>();

            gl.StartPoint = new Vector2(_gm.Grid.PointX(p.Index), _gm.TilesTall - _gm.Grid.PointY(p.Index) + _gm.TilesTall - _gm.Grid.CountEmptyInColumn(GlobalCombat.GM.Grid.PointX(p.Index)));
            gl.NextPoint = new Vector2(_gm.Grid.PointX(p.Index), _gm.TilesTall - _gm.Grid.PointY(p.Index));
            gl.name = "gem" + p.TileID;
            gl.TileID = p.TileID;
            gl.hasGravity = true;
            gl.TravelTime = 0.7f;
            gl.CurrentColorType = p.ColorType;

            gl.OnGemClick += _gm.GetGemClick;
            gl.OnGemDrag += _gm.GetGemDrag;
            gem.transform.localPosition = gl.StartPoint;
            gem.transform.SetParent(_gm.Gameboard.transform, false);
        }

        GlobalCombat.GM.UpdateTileColors();

    }

    public void LoadTeams()
    {
        GoodTeam();
        BadTeam();

        _gm.DisplayTeams();
    }
    private void GoodTeam()
    {
        oPlayer Good = new oPlayer();
        Good.Name = "Good";
        Good.TeamType = TeamType.Good;

        oUnit m1 = new oUnit("Angel", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m1.ColorIdentity = ColorIdentity.RedBlue;
        m1.CardImage = CardImage.darklancer;
        m1.AddSkill(new oUnitSkill("Fly", SkillType.Combat, 6, "flying"));
        Good.Team.Add(m1);

        oUnit m2 = new oUnit("Demon", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m2.ColorIdentity = ColorIdentity.RedBrown;
        m2.CardImage = CardImage.swordsman;
        m2.Exp += 3000;
        m2.AddSkill(new oUnitSkill("Rage", SkillType.Combat, 11, "raging"));
        Good.Team.Add(m2);


        oUnit m3 = new oUnit("Priest", new oRange(3, 7), new oRange(2, 6), new oRange(2, 18), new oRange(2, 7));
        m3.ColorIdentity = ColorIdentity.RedGreen;
        m3.CardImage = CardImage.cleric;
        m3.Exp += 7000;
        m3.AddSkill(new oUnitSkill("Bless", SkillType.Combat, 6, "Blessing"));
        Good.Team.Add(m3);

        _gm.Players.Add(Good);
    }
    private void BadTeam()
    {
        oPlayer bad = new oPlayer();
        bad.Name = "Bad";
        bad.TeamType = TeamType.Bad;

        oUnit m1 = new oUnit("Hunk1", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m1.ColorIdentity = ColorIdentity.GreenBrown;
        m1.CardImage = CardImage.battlehunk;
        m1.AddSkill(new oUnitSkill("Flex", SkillType.Combat, 6, "flexing"));
        bad.Team.Add(m1);

        oUnit m2 = new oUnit("Hunk2", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m2.ColorIdentity = ColorIdentity.GreenBrown;
        m2.CardImage = CardImage.battlehunk;
        m2.Exp += 3000;
        m2.AddSkill(new oUnitSkill("Flex", SkillType.Combat, 11, "flexing"));
        bad.Team.Add(m2);


        oUnit m3 = new oUnit("Priest", new oRange(3, 7), new oRange(2, 6), new oRange(2, 18), new oRange(2, 7));
        m3.ColorIdentity = ColorIdentity.RedGreen;
        m3.CardImage = CardImage.cleric;
        m3.Exp += 7000;
        m3.AddSkill(new oUnitSkill("Bless", SkillType.Combat, 6, "Blessing"));
        bad.Team.Add(m3);


        _gm.Players.Add(bad);
    }

}