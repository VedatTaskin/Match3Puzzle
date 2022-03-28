using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColumnRocket : Bombs
{
    public GameObject columnRocketGO;
    public override BombType bombType => BombType.ColumnBomb;
    public override bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {

        switch (otherGamepiece.gamepieceType)
        {
            case GamepieceType.Normal:
                anyMatches = ColumnVsNormal(gamepiece, board, otherGamepiece);
                break;

            case GamepieceType.Collectible:
                break;

            case GamepieceType.Changeable:
                break;

            case GamepieceType.Bomb:

                switch (otherGamepiece.bombType)
                {
                    case BombType.RowBomb:
                        anyMatches = ColumnVsRow(gamepiece, board, otherGamepiece);
                        break;
                    case BombType.ColumnBomb:
                        anyMatches = ColumnVsColumn(gamepiece, board, otherGamepiece);
                        break;
                    default:
                        break;
                }
                break;


            case GamepieceType.NotMoveable:
                break;

            default:
                break;
        }

        return anyMatches;
    }

    private bool ColumnVsColumn(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }

    private bool ColumnVsRow(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> bombedPieces = new List<Gamepiece>();

        return false;

    }

    private bool ColumnVsNormal(Gamepiece bomb, Board board, Gamepiece other)
    {
        StartCoroutine(SelfDestroy(board));

        var matches= CheckNormalMatches(bomb, board, other);

        if (matches.Count != 0 || matches != null)
        {
            board.gamepieceData.ClearGamepieces(matches);
        }
        return true;

    }
    
    private List<Gamepiece> CheckNormalMatches(Gamepiece bomb, Board board, Gamepiece other)
    {
        List<Gamepiece> matches = board.gamepieceData.FindMatchesAt(other.xIndex, other.yIndex);
        GameObject newBomb=null;
        // if number of matches greater than 4 we create bomb
        if (matches.Count >= 4)
        {
            Vector2 swapDirection = new Vector2(other.xIndex - bomb.xIndex, other.yIndex - bomb.yIndex);
            newBomb= board.DropBomb(other.xIndex, other.yIndex, swapDirection, matches);
        }

        if (newBomb != null)
        {
            matches.Remove(newBomb.GetComponent<Gamepiece>());
        }

        return matches;        
    }
    
    public override IEnumerator SelfDestroy(Board board,Gamepiece otherGamepiece=null)
    {
        //we lock the tiles that column we are in,
        for (int i = 0; i < board.height; i++)
        {
            board.tileData.allTiles[xIndex, i].isLockedAgainstCollapse = true;
        }

        HideMySelf();
        var position = new Vector3(xIndex, yIndex, -2);
        Instantiate(columnRocketGO, position, Quaternion.identity);
        yield return null;

    }

    void HideMySelf()
    {
        board.gamepieceData.allGamepieces[xIndex, yIndex] = null;
        board.gamepieceData.BreakTilesAt(xIndex, yIndex);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.position = new Vector3(100, 100);
        Destroy(gameObject, 5f);
    }
    
    
}