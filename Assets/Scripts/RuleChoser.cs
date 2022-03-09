using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RuleChoser 
{


    public static List<Gamepiece> ChooseRule(Gamepiece clicked, Gamepiece target, Board board)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        // Bomb- Bomb
        if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType == GamepieceType.Bomb)
        {
            return bombedPieces = ApplyTwoBombRule(clicked, board, target);
        }

        // Bomb-Normal
        if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType == GamepieceType.Normal)
        {
            bombedPieces = BombVsNormal(clicked, board, target);
            List<Gamepiece> matchesAtTargetGamepiece = board.gamepieceData.FindMatchesAt(target.xIndex, target.yIndex);
            return bombedPieces.Union(matchesAtTargetGamepiece).ToList();
        }

        // Normal-Bomb
        if (clicked.gamepieceType == GamepieceType.Normal && target.gamepieceType == GamepieceType.Bomb)
        {
            bombedPieces = BombVsNormal(target, board, clicked);
            List<Gamepiece> matchesAtClickedGamepiece = board.gamepieceData.FindMatchesAt(clicked.xIndex, clicked.yIndex);
            return bombedPieces.Union(matchesAtClickedGamepiece).ToList();
        }

        // Bomb- Collectible 
        if (clicked.gamepieceType == GamepieceType.Bomb && target.gamepieceType == GamepieceType.Collectible)
        {
            return bombedPieces = BombVsCollectible(clicked, board, target);
        }

        // Collectible-Bomb 
        if (clicked.gamepieceType == GamepieceType.Collectible && target.gamepieceType == GamepieceType.Bomb)
        {
            return bombedPieces = BombVsCollectible(target, board, clicked);
        }

        // Changeable- changeable
        if (clicked.gamepieceType == GamepieceType.Changeable || target.gamepieceType == GamepieceType.Changeable)
        {

            // changable var, 
        }

        // collectible-collectible
        if (clicked.gamepieceType == GamepieceType.Collectible || target.gamepieceType == GamepieceType.Collectible)
        {
            // collectible var
        }

        // Normal- Collectible 
        if (clicked.gamepieceType == GamepieceType.Normal && target.gamepieceType == GamepieceType.Collectible)
        {
            return bombedPieces = NormalVsCollectible(clicked, board, target);
        }

        // Collectible-Normal 
        if (clicked.gamepieceType == GamepieceType.Collectible && target.gamepieceType == GamepieceType.Normal)
        {
            return bombedPieces = NormalVsCollectible(target, board, clicked);
        }

        //Normal- Normal
        if (clicked.gamepieceType == GamepieceType.Normal && target.gamepieceType == GamepieceType.Normal)
        {
            return NormalVsNormal(clicked, target, board);
        }

        return null;
    }

    private static List<Gamepiece> NormalVsNormal(Gamepiece clicked, Gamepiece target, Board board)
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

    private static List<Gamepiece> BombVsNormal(Gamepiece bomb, Board board, Gamepiece otherGamepiece)
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
                bombedPieces = bombedPieces.Union(BombVsNormal(piece, board,otherGamepiece)).ToList();
            }
        }
        return bombedPieces;
    }

    public static List<Gamepiece> ApplyTwoBombRule(Gamepiece bomb, Board board, Gamepiece otherBomb)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        // we should check each bomb blast seperately
        //Color - color
        if (bomb.bombType == BombType.Coloured && otherBomb.bombType == BombType.Coloured)
        {
            bombedPieces = ColorBombVsColorBomb(bomb, board, otherBomb);
        }

        //Color - row
        //Color - column
        //Color - adjacent
        //Color - 
        //....

        return bombedPieces;

    }

    public static List<Gamepiece> ColorBombVsColorBomb(Gamepiece bomb, Board board, Gamepiece otherBomb)
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

    public static List<Gamepiece> BombVsCollectible(Gamepiece bomb, Board board, Gamepiece collectible)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();
        //we are doing nothing
        return bombedPieces;
    }

    public static List<Gamepiece> NormalVsCollectible(Gamepiece normal, Board board, Gamepiece collectible)
    {
        List<Gamepiece> matches = board.gamepieceData.FindMatchesAt(normal.xIndex, normal.yIndex);
        // if number of matches greater than 4 we create bomb
        if (matches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(normal.xIndex - collectible.xIndex, normal.yIndex - collectible.yIndex);
            board.DropBomb(normal.xIndex, normal.yIndex, swapDirection, matches);
        }

        return matches;


    }


}
