using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBomb :Bombs,ISelfDestroy
{
    public override BombType bombType => BombType.Color;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece )
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear= ColorVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        piecesToClear= ColorVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        piecesToClear = ColorVsColumn (gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Adjacent:
                        piecesToClear = ColorVsAdjacent(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Color:
                        piecesToClear = ColorVsColor(gamepiece, board, otherGamepiece);
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

    private List<Gamepiece> ColorVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {

        Debug.Log("Hi");
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

        return matches;
    }

    public List<Gamepiece> ColorVsColor(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var piece = board.gamepieceData.allGamepieces[i, j];
                if (piece != null && !bombedPieces.Contains(piece))
                {
                    bombedPieces.Add(piece);
                }
            }
        }
        return bombedPieces;
    }

    public List<Gamepiece> ColorVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;
    }

    public List<Gamepiece> ColorVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;
    }

    public List<Gamepiece> ColorVsAdjacent(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;
    }

    public override List<Gamepiece> SelfDestroy(Board board, Gamepiece otherGamepiece = null)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        return matches;
    }
}
