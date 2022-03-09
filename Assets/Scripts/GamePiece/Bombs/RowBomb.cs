using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RowBomb : Bombs
{
    public override BombType bombType => BombType.RowBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece = null)
    {
        return FindGamepieceInRow(gamepiece.yIndex);
    }

    public List<Gamepiece> FindGamepieceInRow(int yIndex)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = 0; i <board.width; i++)
        {
            var piece =board.gamepieceData.allGamepieces[i, yIndex];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                matches.Add(piece);
            }
        }
        return matches;
    }

}
