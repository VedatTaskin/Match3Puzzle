public enum GameState
{
    CanSwap,
    Busy
}

public enum PieceState
{
    CanSwap,
    Moving,
    Busy
}


public enum GamepieceType
{
    Normal,
    Collectible,
    Changeable,
    Bomb,
    NotMoveable
}

public enum TileType
{
    Normal,
    Breakable,
    Nonbreakable,
    Obstacle,
    None
}

public enum BombType
{
    RowBomb,
    ColumnBomb,
    Adjacent,
    Color,
    None
}

public enum NormalGamepieceType
{
    Yellow,
    Blue,
    Magenta,
    Indigo,
    Green,
    Teal,
    Red,
    Cyan,
    Wild,
    None
}

public enum MoveType
{
    Swap,
    Fall
}


