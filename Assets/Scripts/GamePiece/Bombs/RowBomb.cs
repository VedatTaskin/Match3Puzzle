﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RowBomb : Bombs
{
    public override BombType bombType => BombType.RowBomb;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear = RowVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        piecesToClear = RowVsRow(gamepiece, board, otherGamepiece);
                        break;
                    default:
                        break;
                }
                break;


            case GamepieceType.NotMoveable:
                break;

            default:
                break;
        }

        return piecesToClear;
    }

    private List<Gamepiece> RowVsRow(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        return matches;
    }

    private List<Gamepiece> RowVsNormal(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);

        for (int i = 0; i < board.width; i++)
        {
            var piece = board.gamepieceData.allGamepieces[i, yIndex];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                if (piece.gamepieceType == GamepieceType.Bomb)
                {
                    var effectedGamepieces = piece.GetComponent<ISelfDestroy>().SelfDestroy(board).ToList();
                    if (effectedGamepieces != null)
                    {
                        matches = matches.Union(effectedGamepieces).ToList();
                    }
                }
                else
                {
                    matches.Add(piece);
                }

            }
        }
        return matches;

    }


    public override List<Gamepiece> SelfDestroy(Board board)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);

        for (int i = 0; i < board.width; i++)
        {
            var piece = board.gamepieceData.allGamepieces[i, yIndex];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                if (piece.gamepieceType == GamepieceType.Bomb)
                {
                    var effectedGamepieces= piece.GetComponent<ISelfDestroy>().SelfDestroy(board).ToList();
                    if (effectedGamepieces!= null)
                    {
                        matches = matches.Union(effectedGamepieces).ToList();
                    }                    
                }
                else
                {
                    matches.Add(piece);
                }
            }
        }
        return matches;
    }
}
