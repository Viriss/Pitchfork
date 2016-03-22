
public class oStateTop : oStateMachine
{

    public override void Entry()
    {
    }
    public override void Active()
    {
        GlobalCombat _gm = GlobalCombat.GM;

        //flip turn
        if (!_gm.HasExtraTurn && !_gm.StartOfGame)
        {
            switch (_gm.TeamTurn)
            {
                case "Good":
                    _gm.TeamTurn = "Bad";
                    break;
                case "Bad":
                    GlobalCombat.GM.TeamTurn = "Good";
                    break;
            }
        }
        GlobalCombat.GM.UpdateTurnIndicator();
        GlobalCombat.GM.HasExtraTurn = false;
        GlobalCombat.GM.StartOfGame = false;

        if (GlobalCombat.GM.TeamTurn == "Good")
        {
            GlobalCombat.GM.AddState(typeof(oStateActionUser));
        }
        else
        {
            GlobalCombat.GM.AddState(typeof(oStateActionAI));
        }
                
        this.State = MachineState.Exit;
    }
    public override void Exit()
    {
    }
        
}

