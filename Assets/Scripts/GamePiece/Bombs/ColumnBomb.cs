using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBomb : Bombs
{
    public override BombType bombType => BombType.ColumnBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return board.gamepieceData.FindGamepieceInColumn(gamepiece.xIndex);
    }
}