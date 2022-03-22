using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : Gamepiece , IGamepieceRule
{
    bool anyMatches = false;
 
    public override GamepieceType gamepieceType => GamepieceType.Collectible;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;

    public virtual bool PerformRule(Gamepiece collectible, Board board, Gamepiece otherGamepiece)
    {

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = CollectibleVsNormal(collectible, board, otherGamepiece);
                break;
            case GamepieceType.Collectible:
                break;
            default:
                break;
        }
        return anyMatches;
    }

    private bool CollectibleVsNormal(Gamepiece collectible, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);

        return anyMatches;
    }
}
