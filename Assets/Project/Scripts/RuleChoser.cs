using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleChoser 
{
    public static bool ChooseRule(Gamepiece clicked, Gamepiece target, Board board)
    {

        //Bomb 
        if (clicked.gamepieceType == GamepieceType.Bomb || target.gamepieceType == GamepieceType.Bomb)
        {
            if (clicked.bombType == BombType.Color || target.bombType == BombType.Color)             // we assign the job to the color bomb
            {
                return ChooseOneOfTheBombs(clicked, target, board, BombType.Color);
            }
            else if (clicked.bombType == BombType.Adjacent || target.bombType == BombType.Adjacent)
            {
                return ChooseOneOfTheBombs(clicked, target, board, BombType.Adjacent);
            }
            else if (clicked.bombType == BombType.ColumnBomb || target.bombType == BombType.ColumnBomb)
            {
                return ChooseOneOfTheBombs(clicked, target, board, BombType.ColumnBomb);
            }
            else if (clicked.bombType == BombType.RowBomb || target.bombType == BombType.RowBomb)
            {
                return ChooseOneOfTheBombs(clicked, target, board, BombType.RowBomb);
            }
            return false;
        }

        //Collectible
        else if(clicked.gamepieceType == GamepieceType.Collectible || target.gamepieceType == GamepieceType.Collectible)
        {

            if (clicked.gamepieceType == GamepieceType.Collectible)
            {
                IGamepieceRule rule = clicked.GetComponent<IGamepieceRule>();
                if (rule != null)
                {
                    return rule.PerformRule(clicked, board, target);
                }
            }
            else
            {
                IGamepieceRule rule = target.GetComponent<IGamepieceRule>();
                if (rule != null)
                {
                    return rule.PerformRule(target, board, clicked);
                }
            }
            return false;
        }

        // Changeable not done
        else if (clicked.gamepieceType == GamepieceType.Changeable || target.gamepieceType == GamepieceType.Changeable)
        {

        }

        //Normal vs Normal
        else if (clicked.gamepieceType == GamepieceType.Normal || target.gamepieceType == GamepieceType.Normal)
        {
            IGamepieceRule rule = clicked.GetComponent<IGamepieceRule>();
            if (rule != null)
            {
                return rule.PerformRule(clicked, board, target);
            }
        }
        return false;
    }

    public static bool ChooseOneOfTheBombs(Gamepiece clicked, Gamepiece target, Board board, BombType bombType)
    {

        if (clicked.bombType == bombType)
        {
            IGamepieceRule rule = clicked.GetComponent<IGamepieceRule>();
            if (rule != null)
            {
                return rule.PerformRule(clicked, board, target);
            }
        }
        else
        {
            IGamepieceRule bombRule = target.GetComponent<IGamepieceRule>();
            if (bombRule != null)
            {
                return bombRule.PerformRule(target, board, clicked);
            }
        }

        return false;
    }
}
