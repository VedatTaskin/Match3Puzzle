using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalGamepiece : Gamepiece,IGamepieceRule
{
    bool anyMatches = false;
    public override GamepieceType gamepieceType => GamepieceType.Normal;
    public override BombType bombType => BombType.None;
    public virtual bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = NormalVsNormal(gamepiece, board, otherGamepiece);
                break;
            default:
                break;
        }

        return anyMatches;
    }
    public bool NormalVsNormal(Gamepiece clicked, Board board, Gamepiece target)
    {
        List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
        List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);

        List<Gamepiece> allMatches = matchesAtClickedGamepiece.Union(matchesAtTargetGamepiece).ToList();


        if (allMatches.Count != 0 )
        {
            //first we will check if the matches count greater than 4 or not
            // if number of matches greater than 4 we will create a bomb
            if (matchesAtClickedGamepiece.Count >= 4)
            {
                board.gamepieceData.ClearGamepieces(matchesAtClickedGamepiece);
                Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matchesAtClickedGamepiece);
            }
            if (matchesAtTargetGamepiece.Count >= 4)
            {
                board.gamepieceData.ClearGamepieces(matchesAtTargetGamepiece);
                Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                board.DropBomb(target.xIndex, target.yIndex, swapDirection, matchesAtTargetGamepiece);
            }

            if (matchesAtClickedGamepiece.Count==3)
            {
                board.gamepieceData.ClearGamepieces(matchesAtClickedGamepiece);
            }
            if (matchesAtTargetGamepiece.Count == 3)
            {
                board.gamepieceData.ClearGamepieces(matchesAtTargetGamepiece);
            }
            return true;
        }

        return false;

    }

}
