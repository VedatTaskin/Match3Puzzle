using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBomb :Bombs
{
    public override BombType bombType => BombType.Coloured;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece )
    {
        return FindGamepieceSameColor(gamepiece, board, otherGamepiece.normalGamepieceType);
    }

    public List<Gamepiece> FindGamepieceSameColor(Gamepiece gamepiece, Board board, NormalGamepieceType normalType )
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        matches.Add(gamepiece); // our coloured bomb is added

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var piece = board.gamepieceData.allGamepieces[i, j];
                if ( piece != null)
                {
                    if (piece.normalGamepieceType == normalType && !matches.Contains(piece) && piece.gamepieceType != GamepieceType.Collectible)
                    {
                        matches.Add(piece);
                    }
                }
            }
        }

        return matches;
    }



}
