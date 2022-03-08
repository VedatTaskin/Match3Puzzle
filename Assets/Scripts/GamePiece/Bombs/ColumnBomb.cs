using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnBomb : Bombs
{
    public override BombType bombType => BombType.ColumnBomb;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board)
    {
        return FindGamepieceInColumn(gamepiece.xIndex);
    }


    public List<Gamepiece> FindGamepieceInColumn(int xIndex)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = 0; i < board.height; i++)
        {
            var piece = board.gamepieceData.allGamepieces[xIndex, i];

            if (!matches.Contains(piece) && piece != null)
            {
                matches.Add(piece);
            }
        }

        return matches;
    }
}