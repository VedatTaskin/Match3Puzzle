﻿using System.Collections;
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

        return false;

    }
    public bool ColumnVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }
    public bool ColumnVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        StartCoroutine(SelfDestroy(board));

        var matches= CheckNormalMatches(bomb, board, other);

        if (matches.Count != 0 || matches != null)
        {
            board.gamepieceData.ClearGamepieces(matches);
            StartCoroutine(board.CollapseSomePlaces(matches));

        }
        return true;

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
        Debug.Log("cağrıldı" + this.name);
        List<Gamepiece> matches = new List<Gamepiece>();

        int upDirection = yIndex;
        int downDirection = yIndex;

        HideMySelf();

        //Clearing objects in ıp and down direction synchronously
        //objelerin silinme işlemi bittikten sonra collapse çağrılıyor
        for (int i = 0; i < board.height; i++)
        {
            Gamepiece deletedPiece;
            
            if (upDirection < board.height)
            {
                deletedPiece= ClearThisGamepiece(upDirection,otherGamepiece);
                if (!matches.Contains(deletedPiece) )
                {
                    matches.Add(deletedPiece);
                }
                upDirection++;
            }

            if (downDirection >= 0)
            {
                deletedPiece= ClearThisGamepiece(downDirection,otherGamepiece);
                if (!matches.Contains(deletedPiece) )
                {
                    matches.Add(deletedPiece);
                }
                downDirection--;
            }

            yield return new WaitForSeconds(0.1f);
        }

        board.CollapseAtAColumn(xIndex);
        yield return null;
    }
    Gamepiece ClearThisGamepiece(int row, Gamepiece otherGamepiece)
    {

        var tempPiece = board.gamepieceData.allGamepieces[xIndex, row];

        if (tempPiece != null)
        {
            if ( tempPiece.pieceState == PieceState.CanMove)
            {
                if (tempPiece.gamepieceType == GamepieceType.Bomb )
                {
                    StartCoroutine(tempPiece.GetComponent<ISelfDestroy>().SelfDestroy(board, this));
                }
                else
                {                    
                    board.gamepieceData.ClearGamepieceAt(xIndex, row);
                    return tempPiece;
                }
            }
        }
        return null;
    }
    void HideMySelf()
    {
        board.gamepieceData.allGamepieces[xIndex, yIndex] = null;
        board.gamepieceData.BreakTilesAt(xIndex, yIndex);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.position = new Vector3(100, 100);
        Destroy(gameObject, 5f);
    }
}