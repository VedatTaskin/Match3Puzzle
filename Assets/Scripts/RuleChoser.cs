using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleChoser 
{
    public static List<Gamepiece> ChooseRule(Gamepiece clicked, Gamepiece target, Board board)
    {
        List<Gamepiece> piecesToClear = new List<Gamepiece>();

        //Bomb 
        if (clicked.gamepieceType == GamepieceType.Bomb || target.gamepieceType == GamepieceType.Bomb)
        {
            if (clicked.bombType == BombType.Color || target.bombType == BombType.Color)             // we assign the job to the color bomb
            {
                return piecesToClear= ChoosePriorityBomb(clicked, target, board, BombType.Color);
            }
            else if (clicked.bombType == BombType.Adjacent || target.bombType == BombType.Adjacent)
            {
                return piecesToClear = ChoosePriorityBomb(clicked, target, board, BombType.Adjacent);
            }
            else if (clicked.bombType == BombType.ColumnBomb || target.bombType == BombType.ColumnBomb)
            {
                return piecesToClear = ChoosePriorityBomb(clicked, target, board, BombType.ColumnBomb);

            }
            else if (clicked.bombType == BombType.RowBomb || target.bombType == BombType.RowBomb)
            {
                return piecesToClear = ChoosePriorityBomb(clicked, target, board, BombType.RowBomb);
            }
        }

        //Collectible not done
        else if(clicked.gamepieceType == GamepieceType.Collectible || target.gamepieceType == GamepieceType.Collectible)
        {

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
                return piecesToClear = rule.PerformRule(clicked, board, target);
            }

        }

        return null;
    }

    public static List<Gamepiece> ChoosePriorityBomb(Gamepiece clicked, Gamepiece target, Board board, BombType bombType)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        if (clicked.bombType == bombType)
        {
            IGamepieceRule rule = clicked.GetComponent<IGamepieceRule>();
            if (rule != null)
            {
                return bombedPieces = rule.PerformRule(clicked, board, target);
            }
        }
        else
        {
            IGamepieceRule bombRule = target.GetComponent<IGamepieceRule>();
            if (bombRule != null)
            {
                return bombedPieces = bombRule.PerformRule(target, board, clicked);
            }
        }
        return null;
    }
}
