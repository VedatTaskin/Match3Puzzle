using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentBomb : Bombs
{
    public int neighborMultiplier = 1;
    public override BombType bombType => BombType.Adjacent;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board)
    {
        return FindGamepieceInAdjacent(gamepiece);
    }

    public List<Gamepiece> FindGamepieceInAdjacent(Gamepiece gamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = gamepiece.xIndex-neighborMultiplier; i <= gamepiece.xIndex+neighborMultiplier; i++)
        {
            for (int j = gamepiece.yIndex-neighborMultiplier; j <= gamepiece.yIndex+neighborMultiplier; j++)
            {
                if (board.IsWithInBounds(i,j))
                {
                    var piece = board.gamepieceData.allGamepieces[i, j];

                    if (!matches.Contains(piece) && piece != null)
                    {
                        matches.Add(piece);
                    }
                }
            }
        }
        return matches;
    }
}
