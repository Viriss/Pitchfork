using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oStateActionAI : oStateMachine
{
    public override void Entry()
    {
        DetermineAction();
    }
    public override void Active()
    {

    }
    public override void Exit()
    {

    }

    private void DetermineAction()
    {
        GlobalCombat _gm = GlobalCombat.GM;
        oPlayer player;
        oPlayer opponent;

        if (_gm.TeamTurn == "Good")
        {
            player = _gm.GetPlayerByTeamType(TeamType.Good);
            opponent = _gm.GetPlayerByTeamType(TeamType.Bad);
        }
        else
        {
            player = _gm.GetPlayerByTeamType(TeamType.Bad);
            opponent = _gm.GetPlayerByTeamType(TeamType.Good);
        }

        if (player.AnyUnitReadyToFire())
        {
            oUnit unit = null;
            foreach(oUnit u in player.Team)
            {
                if (u.ManaRatio == 1)
                {
                    unit = u;
                    break;
                }
            }

            if (unit != null)
            {
                _gm.DealDamageToFirstUnit(opponent.TeamType, 2);
                unit.EmptyMana();

                _gm.UpdateTeam();

                _gm.AddState(typeof(oStateTop));
                State = MachineState.Exit;
                return;
            }
        }

        StartCoroutine(DoSwap());
    }

    IEnumerator DoSwap()
    {
        GlobalCombat _gm = GlobalCombat.GM;

        //thinking pause
        yield return new WaitForSeconds(_gm.TravelTime);

        oSwapPoints sp = _gm.Grid.BestMove();

        oStateDoMatch xx;
        xx = gameObject.AddComponent(typeof(oStateDoMatch)) as oStateDoMatch;
        xx.GemIndexA = sp.IndexA;
        xx.GemIndexB = sp.IndexB;
        _gm.AddState(xx);
        State = MachineState.Exit;
    }
}

