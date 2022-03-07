using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBomb : SpecialGamepiece
{
    public override SpecialGamepieceType specialGamepieceType => SpecialGamepieceType.ColumnBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return board.gamepieceData.FindGamepieceInRow(gamepiece.yIndex);
    }
}