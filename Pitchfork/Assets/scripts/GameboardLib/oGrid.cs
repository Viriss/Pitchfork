using System;
using System.Collections;
using System.Collections.Generic;

public class oGrid
{
    public int Height;
    public int Width;
    public List<oGridPoint> Points;

    #region "Constructors"
    public oGrid()
    {
        Height = 0;
        Width = 0;
        Points = new List<oGridPoint>();
    }
    public oGrid(int Height, int Width)
        : this()
    {
        this.Height = Height;
        this.Width = Width;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Points.Add(new oGridPoint(IndexFromCoor(x, y)));
            }
        }
    }
    #endregion

    #region "Public Methods"
    #region "Construction"
    public void Create()
    {
        Random rnd = new Random();
        int colorCount = Enum.GetValues(typeof(ColorType)).Length - 1;

        //colorCount = 4;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Points[IndexFromCoor(x, y)].ColorType = (ColorType)rnd.Next(colorCount) + 1;
            }
        }
    }
    public void CreateStableGrid()
    {
        while (true)
        {
            Create();
            if (FindSolutions().Count == 0)
            {
                if (FindMoves().Count > 0)
                {
                    break;
                }
            }
        }
    }
    #endregion

    public oSwapPoints BestMove()
    {
        int bestScore = 0;
        oSwapPoints bestMove = new oSwapPoints();

        foreach (oSwapPoints sp in FindMoves())
        {
            if (sp.Score > bestScore)
            {
                bestScore = sp.Score;
                bestMove = sp;
            }
        }

        return bestMove;        
    }
    public int CountEmptyInColumn(int ColumnID)
    {
        int result = 0;
        foreach(oGridPoint p in Points)
        {
            if (p.ColorType == ColorType.empty && PointX(p.Index) == ColumnID)
            {
                result += 1;
            }
        }
        return result;
    }
    public void DestroyBooms()
    {
        foreach (oGridPoint p in Points)
        {
            if (p.isBoom)
            {
                p.Reset();
                p.ColorType = ColorType.empty;
                p.isBoom = false;
            }
        }
    }
    public void FillEmpty()
    {
        Random rnd = new Random();
        int colorCount = Enum.GetValues(typeof(ColorType)).Length - 1;

        foreach (oGridPoint p in Points)
        {
            if (p.ColorType == ColorType.empty)
            {
                p.ColorType = (ColorType)rnd.Next(colorCount) + 1;
            }
        }
    }
    public List<oSwapPoints> FindMoves()
    {
        ColorType colorSwap;
        List<oSwapPoints> moves = new List<oSwapPoints>();
        List<int> solutions = new List<int>();
        int indexA;
        int indexB;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                List<oGridPoint> tmpGrid = new List<oGridPoint>(Points);
                indexA = IndexFromCoor(x, y);

                //try swap right
                if (indexA > -1 && x < Width - 1)
                {
                    indexB = IndexFromCoor(x + 1, y);

                    if (indexB > -1)
                    {
                        colorSwap = tmpGrid[indexA].ColorType;
                        if (colorSwap != tmpGrid[indexB].ColorType)
                        {
                            SwapTiles(indexA, indexB, tmpGrid);

                            solutions = FindSolutions(tmpGrid);
                            if (solutions.Count > 0)
                            {
                                oSwapPoints sp = new oSwapPoints();
                                if (solutions.Contains(indexA) || solutions.Contains(indexB))
                                {
                                    sp.IndexA = indexA;
                                    sp.Score = tmpGrid[indexA].Score();
                                    sp.ColorType = tmpGrid[indexA].ColorType;

                                    sp.IndexB = indexB;
                                    if (tmpGrid[indexB].Score() > sp.Score)
                                        sp.Score = tmpGrid[indexB].Score();
                                    sp.ColorType = tmpGrid[indexB].ColorType;
                                }

                                moves.Add(sp);
                            }

                            //put them back
                            SwapTiles(indexA, indexB, tmpGrid);
                        }
                    }
                }

                //try swap down
                if (indexA > -1 && y < Height - 1)
                {
                    indexB = IndexFromCoor(x, y + 1);

                    if (indexB > -1)
                    {
                        colorSwap = tmpGrid[indexA].ColorType;
                        if (colorSwap != tmpGrid[indexB].ColorType)
                        {
                            SwapTiles(indexA, indexB, tmpGrid);

                            solutions = FindSolutions(tmpGrid);
                            if (solutions.Count > 0)
                            {
                                oSwapPoints sp = new oSwapPoints();
                                if (solutions.Contains(indexA) || solutions.Contains(indexB))
                                {
                                    sp.IndexA = indexA;
                                    sp.Score = tmpGrid[indexA].Score();
                                    sp.ColorType = tmpGrid[indexA].ColorType;

                                    sp.IndexB = indexB;
                                    if (tmpGrid[indexB].Score() > sp.Score)
                                        sp.Score = tmpGrid[indexB].Score();
                                    sp.ColorType = tmpGrid[indexB].ColorType;
                                }

                                moves.Add(sp);
                            }

                            //put them back
                            SwapTiles(indexA, indexB, tmpGrid);
                        }
                    }
                }

            }
        }
        return moves;
    }
    public List<int> FindSolutions() { return FindSolutions(Points); }
    public List<int> FindSolutions(List<oGridPoint> pointList)
    {
        List<int> result = new List<int>();

        ResetPoints(pointList);

        foreach (oGridPoint p in pointList)
        {
            ScanDirection(p, Direction.Bottom);
            ScanDirection(p, Direction.Top);
            ScanDirection(p, Direction.Left);
            ScanDirection(p, Direction.Right);
        }

        foreach (oGridPoint p in pointList)
        {
            if (p.Maximum() > 2) { result.Add(p.Index); }
        }

        return result;
    }
    public void BoomSolutions()
    {
        foreach(int x in FindSolutions())
        {
            Points[x].isBoom = true;
        }
    }
    public void DestroyGem()
    {
        //TODO: boom a single gem
    }
    public void ExplodeGem()
    {
        //TODO: explode a gem and those around it

    }
    public int GetScore()
    {
        int result = 0;

        foreach(oGridPoint p in Points)
        {
            if (p.isBoom && p.ColorType != ColorType.empty)
            {
                result += p.Score();
            }
        }

        return result;
    }
    public Hashtable GetScoreHash()
    {
        Hashtable score = new Hashtable();
        foreach (oGridPoint p in Points)
        {
            if (p.isBoom && p.ColorType != ColorType.empty)
            {
                if (score.Contains(p.ColorType))
                {
                    score[p.ColorType] = (int)score[p.ColorType] + 1;
                }
                else
                {
                    score.Add(p.ColorType, 1);
                }
            }
        }
        return score;
    }
    public oGridPoint GetPointFromCoor(int X, int Y)
    {
        return Points[IndexFromCoor(X, Y)];
    }
    public oGridPoint GetPointFromTileID(int TileID)
    {
        foreach (oGridPoint p in Points)
        {
            if (p.TileID == TileID) { return p; }
        }
        return null;
    }
    public List<oGridPoint> GetPointsInColumn(int ColumnID)
    {
        List<oGridPoint> result = new List<oGridPoint>();
        foreach (oGridPoint point in Points)
        {
            if (PointX(point.Index) == ColumnID)
            {
                result.Add(point);
            }
        }
        return result;
    }
    public int IndexFromCoor(int X, int Y)
    {
        if (X < 0 || X >= Width)
        {
            return -1;
        }
        if (Y < 0 || Y >= Height)
        {
            return -1;
        }
        return X + (Y * Width);
    }
    public bool IsNextTo(int PointA, int PointB)
    {
        if (PointX(PointA) - 1 == PointX(PointB) && PointY(PointA) == PointY(PointB)) return true;
        if (PointX(PointA) + 1 == PointX(PointB) && PointY(PointA) == PointY(PointB)) return true;
        if (PointY(PointA) - 1 == PointY(PointB) && PointX(PointA) == PointX(PointB)) return true;
        if (PointY(PointA) + 1 == PointY(PointB) && PointX(PointA) == PointX(PointB)) return true;

        return false;
    }
    public oGridPoint PointInDirection(int index, Direction direction) { return PointInDirection(index, direction, Points); }
    public oGridPoint PointInDirection(int index, Direction direction, List<oGridPoint> pointList)
    {
        int x;
        int y;
        int newIndex = 0;

        x = index % Width;
        y = (index - x) / Width;

        switch (direction)
        {
            case Direction.Right:
                newIndex = IndexFromCoor(x + 1, y);
                break;
            case Direction.Bottom:
                newIndex = IndexFromCoor(x, y + 1);
                break;
            case Direction.Left:
                newIndex = IndexFromCoor(x - 1, y);
                break;
            case Direction.Top:
                newIndex = IndexFromCoor(x, y - 1);
                break;
        }

        if (newIndex == -1) { return null; }

        return pointList[newIndex];
    }
    public int PointX(int Index)
    {
        return Index % Width;
    }
    public int PointY(int Index)
    {
        return (Index - PointX(Index)) / Width;
    }
    public void Scramble()
    {

    }
    public void ShiftDown()
    {
        for (int x = 0; x < Width; x++)
        {
            ShiftDownColumn(x);
        }
    }
    public void SwapTiles(int SwapA, int SwapB, List<oGridPoint> points)
    {
        ColorType tmp = ColorType.empty;
        int TileID = 0;

        tmp = points[SwapA].ColorType;
        TileID = points[SwapA].TileID;

        points[SwapA].ColorType = points[SwapB].ColorType;
        points[SwapB].ColorType = tmp;

        points[SwapA].TileID = points[SwapB].TileID;
        points[SwapB].TileID = TileID;

    }
    #endregion

    #region "Private Methods"
    private void ScanDirection(oGridPoint point, Direction direction) { ScanDirection(point, direction, Points); }
    private void ScanDirection(oGridPoint point, Direction direction, List<oGridPoint> pointList)
    {
        oGridPoint next = PointInDirection(point.Index, direction, pointList);

        if (next != null)
        {
            if (point.ColorType == next.ColorType && point.ColorType != ColorType.empty)
            {
                switch (direction)
                {
                    case Direction.Bottom:
                        point.AddDirection(Direction.Bottom);
                        next.AddDirection(Direction.Top);
                        break;
                    case Direction.Top:
                        point.AddDirection(Direction.Top);
                        next.AddDirection(Direction.Bottom);
                        break;
                    case Direction.Left:
                        point.AddDirection(Direction.Left);
                        next.AddDirection(Direction.Right);
                        break;
                    case Direction.Right:
                        point.AddDirection(Direction.Right);
                        next.AddDirection(Direction.Left);
                        break;
                }

                ScanDirection(next, direction, pointList);
            }
        }
    }
    private void ResetPoints() { ResetPoints(Points); }
    private void ResetPoints(List<oGridPoint> pointList)
    {
        foreach (oGridPoint p in pointList) { p.Reset(); }
    }
    private void ShiftDownColumn(int column)
    {
        bool isClean = true;
        int indexA;
        int indexB;
        try
        {
            for (int y = 0; y < Height; y++)
            {
                indexA = IndexFromCoor(column, y);
                indexB = IndexFromCoor(column, y + 1);
                if (indexA > -1 && indexB > -1)
                {
                    if (Points[indexA].ColorType != ColorType.empty && Points[indexB].ColorType == ColorType.empty)
                    {
                        SwapTiles(indexA, indexB, Points);
                        isClean = false;
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        if (!isClean)
        {
            ShiftDownColumn(column);
        }
    }
    #endregion

}
