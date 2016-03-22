using System.Collections;
using UnityEngine;

public class oStateUserAction: oStateMachine
{
    private int GemIndexA;
    private Vector2 DragStart;
    private bool UserReady;
    private GlobalCombat _gm;

    public override void Entry()
    {
        GemIndexA = -1;
        UserReady = true;

        _gm = GlobalCombat.GM;

        _gm.OnClickGem += GetGemClick;
        _gm.OnDragGem += GetGemDrag;
    }
    public override void Active()
    {

    }
    public override void Exit()
    {
        _gm.OnClickGem -= GetGemClick;
        _gm.OnDragGem -= GetGemDrag;
    }

    public void GetGemClick(int TileID)
    {
        oGridPoint tmp = _gm.GetPointByTileID(TileID);

        if (!UserReady) { return; }

        if (GemIndexA == -1)
        {
            GemIndexA = tmp.Index;
            DragStart = Input.mousePosition;
        }
        else if (GemIndexA == tmp.Index)
        {
            GemIndexA = -1;
        }
        if (GemIndexA != tmp.Index && GemIndexA > -1)
        {
            oStateDoMatch xx;
            xx = gameObject.AddComponent(typeof(oStateDoMatch)) as oStateDoMatch;
            xx.GemIndexA = GemIndexA;
            xx.GemIndexB = tmp.Index;
            _gm.AddState(xx);
            State = MachineState.Exit;
            //StartCoroutine(DoSwap(tmp.Index));
        }
    }
    public void GetGemDrag(int TileID)
    {
        oGridPoint tmp;

        if (TileID == -1 && GemIndexA > -1)
        {
            GetGemClick(TileID);
            TileID = GemIndexA;
        }

        if (GemIndexA > -1)
        {
            tmp = _gm.GetPointByTileID(TileID);
            float x = DragStart.x - Input.mousePosition.x;
            float y = DragStart.y - Input.mousePosition.y;
            oGridPoint nextPoint = null;
            float DragDist = _gm.DragDistance;

            if (x > DragDist)
            {
                nextPoint = _gm.Grid.PointInDirection(tmp.Index, Direction.Left);
            }
            else if (x < -DragDist)
            {
                nextPoint = _gm.Grid.PointInDirection(tmp.Index, Direction.Right);
            }
            else if (y > DragDist)
            {
                nextPoint = _gm.Grid.PointInDirection(tmp.Index, Direction.Bottom);
            }
            else if (y < -DragDist)
            {
                nextPoint = _gm.Grid.PointInDirection(tmp.Index, Direction.Top);
            }

            if (nextPoint != null)
            {
                GetGemClick(nextPoint.TileID);
            }
        }
    }


}
