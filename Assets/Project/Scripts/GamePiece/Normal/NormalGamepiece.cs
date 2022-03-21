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

        // We can make this better, Refactoring can be done
        if (allMatches.Count != 0 )
        {
            switch (matchesAtClickedGamepiece.Count)
            {
                case 3:
                    StartCoroutine(ClearRoutine(board, matchesAtClickedGamepiece));
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    StartCoroutine(ClearRoutine(board, matchesAtClickedGamepiece));
                    // if number of matches greater than 4 we will create a bomb
                    Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                    board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matchesAtClickedGamepiece);
                    break;
                default:
                    break;
            }

            switch (matchesAtTargetGamepiece.Count)
            {
                case 3:
                    StartCoroutine(ClearRoutine(board, matchesAtTargetGamepiece));
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    StartCoroutine(ClearRoutine(board, matchesAtTargetGamepiece));
                    // if number of matches greater than 4 we will create a bomb
                    Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                    board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matchesAtClickedGamepiece);
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }

    IEnumerator ClearRoutine(Board board, List<Gamepiece> matches)
    {
        board.gamepieceData.ClearGamepieces(matches);
        yield return null;
    }
}
