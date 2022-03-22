using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBomb :Bombs, ISelfDestroy
{

    public override BombType bombType => BombType.Color;

    public override bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece )
    {
        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches= ColorVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        anyMatches= ColorVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        anyMatches = ColorVsColumn (gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Adjacent:
                        anyMatches = ColorVsAdjacent(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Color:
                        anyMatches = ColorVsColor(gamepiece, board, otherGamepiece);
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

        return anyMatches;

    }

    private bool ColorVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        matches.Add(bomb); // our coloured bomb is added

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var piece = board.gamepieceData.allGamepieces[i, j];
                if (piece != null)
                {
                    if (piece.normalGamepieceType == other.normalGamepieceType && !matches.Contains(piece) && piece.gamepieceType != GamepieceType.Collectible)
                    {
                        matches.Add(piece);
                    }
                }
            }
        }

        if (matches.Count != 0 || matches != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ColorVsColor(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var piece = board.gamepieceData.allGamepieces[i, j];
                if (piece != null && !matches.Contains(piece) && piece.gamepieceType != GamepieceType.Collectible)
                {
                    matches.Add(piece);
                }
            }
        }
        if (matches.Count != 0 || matches != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ColorVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return anyMatches;
    }

    public bool ColorVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return anyMatches;
    }

    public bool ColorVsAdjacent(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return anyMatches;
    }

    public override bool SelfDestroy(Board board, Gamepiece otherGamepiece = null)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        Debug.Log("Color bomb Self Destroying do nothing");
        return false;
    }
}
