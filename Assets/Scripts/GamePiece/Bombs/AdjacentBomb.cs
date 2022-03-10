using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentBomb : Bombs
{
    public int neighborMultiplier = 1;
    public override BombType bombType => BombType.Adjacent;

    public override List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear = AdjacentVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        piecesToClear = AdjacentVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        piecesToClear = AdjacentVsColumn(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Adjacent:
                        piecesToClear = AdjacentVsAdjacent(gamepiece, board, otherGamepiece);
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

    //public List<Gamepiece> FindGamepieceInAdjacent(Gamepiece gamepiece)
    //{
    //    List<Gamepiece> matches = new List<Gamepiece>();

    //    for (int i = gamepiece.xIndex-neighborMultiplier; i <= gamepiece.xIndex+neighborMultiplier; i++)
    //    {
    //        for (int j = gamepiece.yIndex-neighborMultiplier; j <= gamepiece.yIndex+neighborMultiplier; j++)
    //        {
    //            if (board.IsWithInBounds(i,j))
    //            {
    //                var piece = board.gamepieceData.allGamepieces[i, j];

    //                if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
    //                {
    //                    matches.Add(piece);
    //                }
    //            }
    //        }
    //    }
    //    return matches;
    //}

    public List<Gamepiece> AdjacentVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        for (int i = bomb.xIndex - neighborMultiplier; i <= bomb.xIndex + neighborMultiplier; i++)
        {
            for (int j = bomb.yIndex - neighborMultiplier; j <= bomb.yIndex + neighborMultiplier; j++)
            {
                if (board.IsWithInBounds(i, j))
                {
                    var piece = board.gamepieceData.allGamepieces[i, j];

                    if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
                    {
                        matches.Add(piece);
                    }
                }
            }
        }
        return matches;

    }

    public List<Gamepiece> AdjacentVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;

    }

    public List<Gamepiece> AdjacentVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;

    }

    public List<Gamepiece> AdjacentVsAdjacent(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return bombedPieces;

    }
}
