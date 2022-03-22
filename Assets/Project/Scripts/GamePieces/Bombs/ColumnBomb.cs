using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColumnBomb : Bombs
{

    public override BombType bombType => BombType.ColumnBomb;
    public override bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = ColumnVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        anyMatches = ColumnVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        anyMatches = ColumnVsColumn(gamepiece, board, otherGamepiece);
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
    public bool ColumnVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return anyMatches;

    }
    public bool ColumnVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return anyMatches;

    }
    public bool ColumnVsNormal(Gamepiece bomb, Board board, Gamepiece other)
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
                    //var effectedGamepieces = piece.GetComponent<ISelfDestroy>().SelfDestroy(board, this).ToList();
                    //if (effectedGamepieces != null)
                    //{
                    //    matches = matches.Union(effectedGamepieces).ToList();
                    //}
                }
                else
                {
                    matches.Add(piece);
                }
            }
        }

        // check if other Normal gamepiece makes a match3
        matches = matches.Union(CheckNormalMatches(bomb, board, other)).ToList();

        if (matches.Count != 0 || matches != null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private List<Gamepiece> CheckNormalMatches(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);

        // if number of matches greater than 4 we create bomb
        if (matches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(other.xIndex - bomb.xIndex, other.yIndex - bomb.yIndex);
            board.DropBomb(other.xIndex, other.yIndex, swapDirection, matches);
        }

        return matches;
    }
    public override IEnumerator SelfDestroy(Board board,Gamepiece otherGamepiece=null)
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
                    piece.GetComponent<ISelfDestroy>().SelfDestroy(board, this);
                }
                else
                {
                    matches.Add(piece);
                }
            }
        }

        if (matches.Count != 0)
        {
            Debug.Log("Self Destroying");
            //board.gamepieceData.ClearGamepieces(matches);
            //StartCoroutine(board.CollapseRoutine(matches));
            //return true;
        }
        yield return null;
    }
}