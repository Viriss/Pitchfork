public class oGridPoint
{
    public int Index;
    public int TileID;
    public ColorType ColorType;
    public int Left;
    public int Right;
    public int Top;
    public int Bottom;
    public bool isBoom;

    public oGridPoint()
    {
        Index = 0;
        TileID = 0;
        ColorType = ColorType.empty;
        Left = 0;
        Right = 0;
        Top = 0;
        Bottom = 0;
        isBoom = false;
    }
    public oGridPoint(int Index)
        : this()
    {
        this.Index = Index;
        this.TileID = Index;
    }

    public void AddDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Bottom:
                Bottom += 1;
                break;
            case Direction.Top:
                Top += 1;
                break;
            case Direction.Left:
                Left += 1;
                break;
            case Direction.Right:
                Right += 1;
                break;
        }
    }
    public int Maximum()
    {
        int result = 0;
        if (Left > result) { result = Left; }
        if (Right > result) { result = Right; }
        if (Bottom > result) { result = Bottom; }
        if (Top > result) { result = Top; }
        return result;
    }
    public void Reset()
    {
        Left = 0;
        Right = 0;
        Top = 0;
        Bottom = 0;
    }
    public int Score()
    {
        int result = 0;
        if (Left > 2) { result += Left; }
        if (Right > 2) { result += Right; }
        if (Bottom > 2) { result += Bottom; }
        if (Top > 2) { result += Top; }
        return result;
    }
    public int Total()
    {
        return (Left + Right + 1) * (Top + Bottom + 1);
    }



}


public enum Direction { Top, Bottom, Left, Right }
public enum ColorType { 
    empty = 0, 
    Red = 1, 
    Green = 2, 
    Blue = 3, 
    Purple = 4,
    Brown = 5, 
    Yellow = 6, 
    Sword = 7 
}

