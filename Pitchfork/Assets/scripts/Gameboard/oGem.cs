using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class oGem
{
    public int Index;
    public int TileID;
    public GameObject GemObject;
    public GemLogic Logic;

    public oGem()
    {
        Index = 0;
        TileID = 0;
        GemObject = null;
        Logic = null;
    }
    public oGem(GameObject obj, GemLogic logic, int TileID)
    {
        this.GemObject = obj;
        this.Logic = logic;
        this.TileID = TileID;
    }
}
