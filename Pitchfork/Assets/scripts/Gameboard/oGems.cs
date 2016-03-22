using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class oGems
{
    public List<oGem> Items;

    public oGems()
    {
        Items = new List<oGem>();
    }

    public void Add(GameObject obj, GemLogic logic, int TileID)
    {
        oGem x = new oGem(obj, logic, TileID);
        x.Index = Items.Count;
        Items.Add(x);
    }
    public oGem GetByTileID(int TileID)
    {
        foreach(oGem g in Items)
        {
            if (g.TileID == TileID) { return g; }
        }
        return null;
    }
    public void SetTravelTime(float Time)
    {
        foreach (oGem g in Items)
        {
            g.Logic.TravelTime = Time;
        }
    }
    public void LockMoving()
    {
        foreach(oGem g in Items)
        {
            g.Logic.StopMoving();
        }
    }
}

