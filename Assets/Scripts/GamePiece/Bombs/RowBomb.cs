using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBomb : SpecialGamepiece
{
    public override SpecialGamepieceType specialGamepieceType => SpecialGamepieceType.RowBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return board.gamepieceData.FindGamepieceInRow(gamepiece.yIndex);

   }
}
