﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColumnBomb : Bombs
{
    public override BombType bombType => BombType.ColumnBomb;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece=null)
    {
        return FindGamepieceInColumn(gamepiece.xIndex);
    }


    public List<Gamepiece> FindGamepieceInColumn(int xIndex)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = 0; i < board.height; i++)
        {
            var piece = board.gamepieceData.allGamepieces[xIndex, i];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                matches.Add(piece);
            }
        }
        return matches;
    }
}