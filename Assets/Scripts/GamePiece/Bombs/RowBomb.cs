using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBomb : Bombs
{
    public override BombType bombType => BombType.RowBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return board.gamepieceData.FindGamepieceInRow(gamepiece.yIndex);

   }
}
