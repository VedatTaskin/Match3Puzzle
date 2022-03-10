using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalGamepiece : Gamepiece,IGamepieceRule
{
    public override GamepieceType gamepieceType => GamepieceType.Normal;
    public override BombType bombType => BombType.None;
    public virtual List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                piecesToClear = NormalVsNormal(gamepiece, board, otherGamepiece);
                break;
            default:
                break;
        }
        return piecesToClear;
    }
    public List<Gamepiece> NormalVsNormal(Gamepiece clicked, Board board, Gamepiece target)
    {
        List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
        List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);

        // if number of matches greater than 4 we create bomb
        if (matchesAtClickedGamepiece.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
            board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matchesAtClickedGamepiece);
        }
        if (matchesAtTargetGamepiece.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
            board.DropBomb(target.xIndex, target.yIndex, swapDirection, matchesAtTargetGamepiece);
        }

        List<Gamepiece> allMatches = matchesAtClickedGamepiece.Union(matchesAtTargetGamepiece).ToList();
        return allMatches;
    }
}
