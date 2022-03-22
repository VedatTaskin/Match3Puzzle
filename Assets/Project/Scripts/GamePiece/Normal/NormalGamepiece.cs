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

            board.gamepieceData.ClearGamepieces(allMatches);

            //We add bomb immediately after cleaning list 
            if (matchesAtClickedGamepiece.Count >= 4)
            {
                BombCreation(clicked, board, target, matchesAtClickedGamepiece);
            }
            if (matchesAtTargetGamepiece.Count >= 4)
            {
                BombCreation(target, board, clicked, matchesAtTargetGamepiece);
            }

            // silinmiş olan gamepiecelerin yerine üssten yenisi düşecek
            // Silinecek elemanların içinden en alttaki elemanları
            // farklı şekilde uyarıyorum ki onlar üstlerini uyarsın,
            StartCoroutine(CollapseRoutine(board,allMatches));
            return true;
        }
        return false;
    }

    void BombCreation(Gamepiece clicked, Board board, Gamepiece target, List<Gamepiece> matches)
    {
        Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
        var bombGO= board.DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matches);
    }

    IEnumerator CollapseRoutine(Board board, List<Gamepiece> matches)
    {
        List<Gamepiece> PiecesAtTheBottomOfMatches = new List<Gamepiece>();

        //önce ilgili kolonlar gruplandırılıyor kendi içerisinde
        var groupByColumn = matches.GroupBy(n => n.xIndex);

        //bir silinme esnasında en üstteki elemanlar hangisi ise bulunuyor
        foreach (var grp in groupByColumn)
        {
            var bottomPiece = grp.OrderByDescending(i => (i.yIndex)).Last();
            if (!PiecesAtTheBottomOfMatches.Contains(bottomPiece))
            {
                PiecesAtTheBottomOfMatches.Add(bottomPiece);
            }
        }

        //now we collapse each column that we have cleared an object
        foreach (var piece in PiecesAtTheBottomOfMatches)
        {
            _ = CollapseGamepieces(piece);
        }
        yield return null;
    }


    //we return which gamepieces are collapsing
    List<Gamepiece> CollapseGamepieces(Gamepiece piece)
    {
        var allGamepieces = board.gamepieceData.allGamepieces;
        int column = piece.xIndex;

        //we want to know which pieces are moving, we will check if they make another match after collapsing
        List<Gamepiece> movingPieces = new List<Gamepiece>();

        for (int i = piece.yIndex; i < board.height-1 ; i++)
        {
            if (allGamepieces[column, i] == null
                && board.tileData.allTiles[column, i].tileType != TileType.Obstacle)
            {
                //Debug.Log(column + ", " + i +" null");

                for (int j = i+1; j < board.height; j++)
                {
                    if (allGamepieces[column, j] != null)
                    {
                        allGamepieces[column, i] = allGamepieces[column, j];
                        allGamepieces[column, j] = null;
                        allGamepieces[column, i].SetCoordinate(column, i);
                        allGamepieces[column, i].Move(column, i, fallTime*(j-i), MoveType.Fall);

                        if (!movingPieces.Contains(allGamepieces[column, i]))
                        {
                            movingPieces.Add(allGamepieces[column, i]);
                        }
                        break;
                    }
                }
            }
        }
        return movingPieces;
    }
}
