using System;
using System.Collections;
using UnityEngine;

public class oStateViewCard : oStateMachine
{
    public Guid UnitGuid;
    private int _buttonClickID;
    GlobalCombat _gm;

    public override void Entry()
    {
        _gm = GlobalCombat.GM;
        _buttonClickID = -1;
        _gm.OnClickCardButton += GetCardButtonClick;
    }
    public override void Active()
    {
        switch(_buttonClickID)
        {
            case 0:
                _gm.HasExtraTurn = true;
                _gm.AddState(typeof(oStateTop));
                State = MachineState.Exit;
                break;
            case 1:
                _gm.GetUnitByGuid(UnitGuid).EmptyMana();
               
                if (_gm.TeamTurn == "Good")
                {
                    _gm.DealDamageToFirstUnit(TeamType.Bad, 2);
                    
                }
                else
                {
                    _gm.DealDamageToFirstUnit(TeamType.Good, 2);
                }
                _gm.UpdateTeam();
                _gm.AddState(typeof(oStateTop));
                State = MachineState.Exit;
                break;
            case 2:
                break;
        }
    }
    public override void Exit()
    {
        _gm.DoCloseCardUI();
        _gm.OnClickCardButton -= GetCardButtonClick;
    }

    public void GetCardButtonClick(int ButtonID)
    {
        _buttonClickID = ButtonID;
    }
}
