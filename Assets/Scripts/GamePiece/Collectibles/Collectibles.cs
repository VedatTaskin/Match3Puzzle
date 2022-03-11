using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : Gamepiece , IGamepieceRule
{
    public override GamepieceType gamepieceType => GamepieceType.Collectible;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;

    public virtual List<Gamepiece> PerformRule(Gamepiece collectible, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear = CollectibleVsNormal(collectible, board, otherGamepiece);
                break;
            case GamepieceType.Collectible:
                break;
            default:
                break;
        }
        return piecesToClear;
    }

    private List<Gamepiece> CollectibleVsNormal(Gamepiece collectible, Board board, Gamepiece other)
    {
        List<Gamepiece> normalMatches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);

        // if number of matches greater than 4 we create bomb
        if (normalMatches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(other.xIndex - collectible.xIndex, other.yIndex - collectible.yIndex);
            board.DropBomb(other.xIndex, other.yIndex, swapDirection, normalMatches);
        }
        return normalMatches;
    }
}
