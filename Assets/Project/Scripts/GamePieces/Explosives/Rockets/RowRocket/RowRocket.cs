﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RowRocket : Bombs
{
    public GameObject RowRocketGO;
    
    public override BombType bombType => BombType.RowBomb;

    public override bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = RowVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        anyMatches = RowVsRow(gamepiece, board, otherGamepiece);
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

    private bool RowVsRow(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        return false;
    }

    private bool RowVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        StartCoroutine(SelfDestroy(board));

        var matches = CheckNormalMatches(bomb, board, other);

        if (matches.Count != 0 || matches != null)
        {
            board.gamepieceData.ClearGamepieces(matches);
        }
        return true;
    }

    List<Gamepiece> CheckNormalMatches(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);
        GameObject newBomb = null;

        // if number of matches greater than 4 we create bomb
        if (matches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(other.xIndex - bomb.xIndex, other.yIndex - bomb.yIndex);
            newBomb= board.DropBomb(other.xIndex, other.yIndex, swapDirection, matches);
        }

        if (newBomb != null)
        {
            matches.Remove(newBomb.GetComponent<Gamepiece>());
        }

        return matches;
    }
    
    //other gamepiece may be a bomb that triggered us
    // we don't want to trigger it recursively
    public override IEnumerator SelfDestroy(Board board,Gamepiece otherGamepiece=null)
    {
        HideMySelf();
        var position = new Vector3(xIndex, yIndex, -2);
        Instantiate(RowRocketGO, position, Quaternion.identity);
        yield return null;
    }

    void HideMySelf()
    {
        board.gamepieceData.allGamepieces[xIndex,yIndex] = null;
        board.gamepieceData.BreakTilesAt(xIndex, yIndex);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        board.CollapseAtAPoint(xIndex);  // boş olan objenin olduğu yerini dolduruyor bu fonksiyon
        transform.position = new Vector3(100, 100);
        Destroy(gameObject, 5f);
    }
}