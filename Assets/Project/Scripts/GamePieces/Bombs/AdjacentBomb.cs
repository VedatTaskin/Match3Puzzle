using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AdjacentBomb : Bombs
{
    public int neighborMultiplier = 1;
    public override BombType bombType => BombType.Adjacent;
    public override bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = AdjacentVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        anyMatches = AdjacentVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        anyMatches = AdjacentVsColumn(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.Adjacent:
                        anyMatches = AdjacentVsAdjacent(gamepiece, board, otherGamepiece);
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
    public bool AdjacentVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }
    public bool AdjacentVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }
    public bool AdjacentVsAdjacent(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }
    public bool AdjacentVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        StartCoroutine(SelfDestroy(board));

        var matches = CheckNormalMatches(bomb, board, other);

        if (matches.Count != 0 || matches != null)
        {
            board.gamepieceData.ClearGamepieces(matches);
            StartCoroutine(board.CollapseSomePlaces(matches));

        }
        return true;

    }
    public override IEnumerator SelfDestroy(Board board, Gamepiece otherGamepiece=null)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        matches.Add(this);

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
                            StartCoroutine(piece.GetComponent<ISelfDestroy>().SelfDestroy(board, this));
                        }
                        else
                        {
                            matches.Add(piece);
                        }
                    }
                }
            }
        }

        if (matches.Count !=0)
        {
            board.gamepieceData.ClearGamepieces(matches);
            StartCoroutine(board.CollapseSomePlaces(matches));
        }
        yield return null;
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
