using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleChoser 
{
    public static List<Gamepiece> ChooseRule(Gamepiece clicked, Gamepiece target, Board board)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        #region There is a bomb in Swap
        if (clicked.gamepieceType == GamepieceType.Bomb || target.gamepieceType == GamepieceType.Bomb)
        {
            // there is bomb 
            if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType != GamepieceType.Bomb)
            {
                bombedPieces = ApplyOneBombRule(clicked, board,target);
                List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);
                return bombedPieces.Union(matchesAtTargetGamepiece).ToList();
            }

            if (clicked.gamepieceType!= GamepieceType.Bomb && target.gamepieceType == GamepieceType.Bomb  )
            {
                bombedPieces = ApplyOneBombRule(target, board, clicked);
                List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
                return bombedPieces.Union(matchesAtClickedGamepiece).ToList();
            }

            if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType == GamepieceType.Bomb )
            {
                return bombedPieces = ApplyTwoBombRule(clicked, board, target);
            }


        }
        #endregion

        // Not done
        if (clicked.gamepieceType == GamepieceType.Changeable || target.gamepieceType == GamepieceType.Changeable)
        {

            // changable var, 
        }

        // Not done
        if (clicked.gamepieceType == GamepieceType.Collectible || target.gamepieceType == GamepieceType.Collectible)
        {
            // collectible var
        }

        #region Both pieces are Normal
        if (clicked.gamepieceType == GamepieceType.Normal && target.gamepieceType == GamepieceType.Normal)
        {
            List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
            List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);

            // if number of matches greater than 4 we create bomb
            if (matchesAtClickedGamepiece.Count >=4 )
            {
                Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matchesAtClickedGamepiece);                
            }
            if ( matchesAtTargetGamepiece.Count >= 4)
            {
                Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
                board.DropBomb(target.xIndex, target.yIndex, swapDirection, matchesAtTargetGamepiece);
            }
            

            List<Gamepiece> allMatches = matchesAtClickedGamepiece.Union(matchesAtTargetGamepiece).ToList();
            return allMatches;
        }
        #endregion

        return null;
    }

    private static List<Gamepiece> ApplyOneBombRule(Gamepiece bomb, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        IBombRule bombRule = bomb.GetComponent<IBombRule>();
        if (bombRule != null)
        {
            bombedPieces = bombRule.PerformRule(bomb, board,otherGamepiece);
        }

        foreach (var piece in bombedPieces)
        {
            if (piece.gamepieceType == GamepieceType.Bomb && piece != bomb)
            {
                bombedPieces = bombedPieces.Union(ApplyOneBombRule(piece, board,otherGamepiece)).ToList();
            }
        }

        return bombedPieces;
    }

    public static List<Gamepiece> CheckForAnotherBomb(Gamepiece piece, Board board, Gamepiece otherGamepiece)
    {
        List<Gamepiece> otherGamepieces = new List<Gamepiece>();


        if (piece.gamepieceType == GamepieceType.Bomb)
        {
            IBombRule bombRule = piece.GetComponent<IBombRule>();

            if (bombRule != null)
            {
                Debug.Log("there is another bomb");
                otherGamepieces =bombRule.PerformRule(piece, board,otherGamepiece); 

            }
        }
        return otherGamepieces;
    }

    public static List<Gamepiece> ApplyTwoBombRule(Gamepiece bomb, Board board, Gamepiece otherBomb)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        // we should check each bomb blast seperately

        //Color - color
        if (bomb.bombType == BombType.Coloured && otherBomb.bombType == BombType.Coloured)
        {
            bombedPieces = ColorVsColorBombState(bomb, board, otherBomb);
        }

        //Color - row
        //Color - column
        //Color - adjacent
        //Color - 

        return bombedPieces;

    }

    public static List<Gamepiece> ColorVsColorBombState(Gamepiece bomb, Board board, Gamepiece otherBomb)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                var piece = board.gamepieceData.allGamepieces[i, j];
                if (piece != null && !bombedPieces.Contains(piece))
                {
                    bombedPieces.Add(piece);
                }
            }
        }
        return bombedPieces;
    }

}
