using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleChoser 
{

    public static List<Gamepiece> Rule(Gamepiece clicked, Gamepiece target, Board board)
    {
        List<Gamepiece> gamepiecesWillClear = new List<Gamepiece>();


        #region There is a bomb in Swap
        if (clicked.gamepieceType == GamepieceType.Bomb || target.gamepieceType == GamepieceType.Bomb)
        {
            // bomb var, 
            if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType != GamepieceType.Bomb)
            {
                IBombRule bombRule = clicked.GetComponent<IBombRule>();
                if (bombRule != null)
                {
                    gamepiecesWillClear = bombRule.PerformRule(clicked);
                }

                List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);

                return gamepiecesWillClear.Union(matchesAtTargetGamepiece).ToList();

            }

            if (clicked.gamepieceType!= GamepieceType.Bomb && target.gamepieceType == GamepieceType.Bomb  )
            {
                IBombRule bombRule = target.GetComponent<IBombRule>();
                if (bombRule != null)
                {
                    gamepiecesWillClear = bombRule.PerformRule(target);
                }

                List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);

                return gamepiecesWillClear.Union(matchesAtClickedGamepiece).ToList();
            }

            if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType == GamepieceType.Bomb )
            {
                Debug.Log("ikisi de bomb");
            }


        }
        #endregion

        // Not finished
        if (clicked.gamepieceType == GamepieceType.Changeable || target.gamepieceType == GamepieceType.Changeable)
        {

            // changable var, 
        }

        // Not finished
        if (clicked.gamepieceType == GamepieceType.Collectible || target.gamepieceType == GamepieceType.Collectible)
        {
            // collectible var
        }

        #region Both pieces are Normal
        if (clicked.gamepieceType == GamepieceType.Normal && target.gamepieceType == GamepieceType.Normal)
        {
            List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
            List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);
            List<Gamepiece> allMatches = matchesAtClickedGamepiece.Union(matchesAtTargetGamepiece).ToList();
            return allMatches;
        }
        #endregion



        return null;


    }
}
