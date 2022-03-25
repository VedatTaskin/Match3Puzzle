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
            //Burada tüm pieceleri aynı anda yok ediyoruz, Bomba türüne göre yok edilecek pieceler farklı şekilde yok olacak
            foreach (var piece in allMatches)
            {
                if (piece != null)
                {
                    board.gamepieceData.ClearGamepieceAt(piece.xIndex, piece.yIndex);
                }
            }

            //We add bomb immediately after cleaning list 
            if (matchesAtClickedGamepiece.Count >= 4)
            {
                board.BombCreation(clicked, target, matchesAtClickedGamepiece);
            }
            if (matchesAtTargetGamepiece.Count >= 4)
            {
                board.BombCreation(target, clicked, matchesAtTargetGamepiece);
            }

            return true;
        }
        return false;
    }
}
