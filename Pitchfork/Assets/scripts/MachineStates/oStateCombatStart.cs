using UnityEngine;
using System.Collections;

public class oStateCombatStart : oStateMachine
{
    public override void Entry()
    {
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

        GlobalCombat.GM.SetTravelTime(GlobalCombat.GM.TravelTime);

        yield return new WaitForSeconds(GlobalCombat.GM.TravelTime);

        GlobalCombat.GM.LockMoving();
        GlobalCombat.GM.UpdateGemPositions();

        State = MachineState.Exit;
    }

    public void PopulateScreen()
    {
        foreach (oGridPoint p in GlobalCombat.GM.Grid.Points)
        {
            GameObject gem = (GameObject)Instantiate(GlobalCombat.GM.Gem); // Transform.Instantiate(Gem);
            GemLogic gl = gem.GetComponent<GemLogic>();

            gl.StartPoint = new Vector2(GlobalCombat.GM.Grid.PointX(p.Index), GlobalCombat.GM.TilesTall - GlobalCombat.GM.Grid.PointY(p.Index) + GlobalCombat.GM.TilesTall - GlobalCombat.GM.Grid.CountEmptyInColumn(GlobalCombat.GM.Grid.PointX(p.Index)));
            gl.NextPoint = new Vector2(GlobalCombat.GM.Grid.PointX(p.Index), GlobalCombat.GM.TilesTall - GlobalCombat.GM.Grid.PointY(p.Index));
            gl.name = "gem" + p.TileID;
            gl.TileID = p.TileID;
            gl.hasGravity = true;
            gl.TravelTime = 0.7f;
            gl.CurrentColorType = p.ColorType;

            gl.OnGemClick += GlobalCombat.GM.GetGemClick;
            gl.OnGemDrag += GlobalCombat.GM.GetGemDrag;
            gem.transform.localPosition = gl.StartPoint;
            gem.transform.SetParent(GlobalCombat.GM.Gameboard.transform, false);
        }

        GlobalCombat.GM.UpdateTileColors();

    }

    public void LoadTeams()
    {
        GoodTeam();
        //BadTeam();
    }
    private void GoodTeam()
    {
        GlobalCombat _gm = GlobalCombat.GM;

        oPlayer Good = new oPlayer();
        Good.Name = "Good";
        Good.TeamType = TeamType.Good;

        oUnit m1 = new oUnit("Angel", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m1.ColorIdentity = ColorIdentity.Yellow;
        m1.AddSkill(new oUnitSkill("Fly", SkillType.Combat, 6, "flying"));
        Good.Team.Add(m1);

        oUnit m2 = new oUnit("Demon", new oRange(2, 5), new oRange(3, 6), new oRange(4, 20), new oRange(1, 5));
        m2.ColorIdentity = ColorIdentity.RedBrown;
        m2.Exp += 3000;
        m2.AddSkill(new oUnitSkill("Rage", SkillType.Combat, 11, "raging"));
        Good.Team.Add(m2);


        _gm.Players.Add(Good);

        _gm.DisplayTeams();
    }

}