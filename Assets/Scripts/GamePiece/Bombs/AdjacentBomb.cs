using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<Gamepiece> AdjacentVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);

        for (int i = bomb.xIndex - neighborMultiplier; i <= bomb.xIndex + neighborMultiplier; i++)
        {
            for (int j = bomb.yIndex - neighborMultiplier; j <= bomb.yIndex + neighborMultiplier; j++)
            {
                if (board.IsWithInBounds(i, j))
                {
                    var piece = board.gamepieceData.allGamepieces[i, j];

                    if (!matches.Contains(piece) && piece != null && piece.gamepieceType != GamepieceType.Collectible)
                    {

                        if (piece.gamepieceType == GamepieceType.Bomb)
                        {
                            var effectedGamepieces = piece.GetComponent<ISelfDestroy>().SelfDestroy(board, this).ToList();
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
            }
        }

        // check if other Normal gamepiece makes a match3
        matches = matches.Union(CheckNormalMatches(bomb, board, other)).ToList();
        return matches;

    }
    public override List<Gamepiece> SelfDestroy(Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);
        if (otherGamepiece != null)
        {
            matches.Add(otherGamepiece);
        }

        for (int i = xIndex - neighborMultiplier; i <= xIndex + neighborMultiplier; i++)
        {
            for (int j = yIndex - neighborMultiplier; j <=yIndex + neighborMultiplier; j++)
            {
                if (board.IsWithInBounds(i, j))
                {
                    var piece = board.gamepieceData.allGamepieces[i, j];

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
            }
        }
        return matches;

    }
    private List<Gamepiece> CheckNormalMatches(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> normalMatches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);

        // if number of matches greater than 4 we create bomb
        if (normalMatches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(other.xIndex - bomb.xIndex, other.yIndex - bomb.yIndex);
            board.DropBomb(other.xIndex, other.yIndex, swapDirection, normalMatches);
        }
        return normalMatches;
    }
}
