using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColumnBomb : Bombs
{

    public override BombType bombType => BombType.ColumnBomb;
    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear = ColumnVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        piecesToClear = ColumnVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        piecesToClear = ColumnVsColumn(gamepiece, board, otherGamepiece);
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
    public List<Gamepiece> ColumnVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;

    }
    public List<Gamepiece> ColumnVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;

    }
    public List<Gamepiece> ColumnVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);

        for (int i = 0; i < board.height; i++)
        {
            var piece = board.gamepieceData.allGamepieces[xIndex, i];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                if (piece.gamepieceType == GamepieceType.Bomb)
                {
                    var effectedGamepieces = piece.GetComponent<ISelfDestroy>().SelfDestroy(board,this).ToList();
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
    public override List<Gamepiece> SelfDestroy(Board board,Gamepiece otherGamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);
        if (otherGamepiece != null)
        {
            matches.Add(otherGamepiece);
        }

        for (int i = 0; i < board.height; i++)
        {
            var piece = board.gamepieceData.allGamepieces[xIndex, i];

            if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
            {
                if (piece.gamepieceType == GamepieceType.Bomb)
                {
                    var effectedGamepieces = piece.GetComponent<ISelfDestroy>().SelfDestroy(board,this).ToList();
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
}