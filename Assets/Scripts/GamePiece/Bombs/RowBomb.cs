using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBomb : Bombs
{
    public override BombType bombType => BombType.RowBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board)
    {
        return FindGamepieceInRow(gamepiece.yIndex);
    }

    public List<Gamepiece> FindGamepieceInRow(int yIndex)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = 0; i <board.width; i++)
        {
            var piece =board.gamepieceData.allGamepieces[i, yIndex];

            if (!matches.Contains(piece) && piece != null)
            {
                matches.Add(piece);
            }
        }
        return matches;
    }

}
