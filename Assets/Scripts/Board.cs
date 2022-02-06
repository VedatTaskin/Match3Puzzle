using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public int borderSize;
    public float swapTime = 0.5f;


    public GameObject tilePrefab;
    public GameObject[] gamePiecePrefabs;

    Tile[,] m_AllTiles;
    GamePiece[,] m_AllGamePieces;

    Tile m_clickedTile;
    Tile m_targetTile;

    private void Start()
    {
        m_AllTiles = new Tile[width, height];
        m_AllGamePieces = new GamePiece[width, height];

        SetupTiles();
        SetupCamera();
        FillRandom();
    }

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile= Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                
                tile.name = "Tile (" + i + "," + j +")";
                
                m_AllTiles[i, j] = tile.GetComponent<Tile>();

                m_AllTiles[i, j].Init(i, j, this);

                tile.transform.parent = this.transform;            
            }

        }
    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3( (float)(width-1) / 2, (float)(height-1) / 2, -10f);

        float aspectRatio = (float) Screen.width / (float) Screen.height;

        float verticalSize = (float) height / 2f + (float) borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    
    }

    GameObject GetRandomGamePiece()
    {
        int randomIndex = Random.Range(0, gamePiecePrefabs.Length);
        if (gamePiecePrefabs[randomIndex] == null)
        {
            Debug.LogWarning("BOARD" + randomIndex + " doesn't contain a vaild Gamepiece prefab");
        }
        return gamePiecePrefabs[randomIndex];
    }

    public void PlaceGamePiece(GamePiece gamePiece,int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD: invalid gamepiece");
            return;
        }
        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;
        if (IsWithInBounds(x,y))
        {
            m_AllGamePieces[x, y] = gamePiece;
        }
        gamePiece.SetCoordinate(x, y);
    }

    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    void FillRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

                if (randomPiece !=null)
                {
                    randomPiece.GetComponent<GamePiece>().Init(this);
                    PlaceGamePiece(randomPiece.GetComponent<GamePiece>(), i, j);
                    randomPiece.transform.parent = transform;
                }
            }
        }
    }

    public void ClickTile(Tile tile)
    {
        if (m_clickedTile ==null)
        {
            m_clickedTile = tile;
            //Debug.Log("clicked tile:" + tile.name);
        }
    }

    public void DragToTile(Tile tile)
    {
        if (m_clickedTile != null && IsNextTo(m_clickedTile, tile))
        {
            m_targetTile = tile;            
        }
    }
    
    public void ReleaseTile()
    {
        if (m_clickedTile != null && m_targetTile != null ) 
        {
            SwitchTiles(m_clickedTile, m_targetTile);
        }
        m_clickedTile = null;
        m_targetTile = null;
    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        GamePiece clickedPiece = m_AllGamePieces[clickedTile.xIndex, clickedTile.yIndex];
        GamePiece targetPiece = m_AllGamePieces[targetTile.xIndex, targetTile.yIndex];

        clickedPiece.Move(targetTile.xIndex, targetTile.yIndex, swapTime);
        targetPiece.Move(clickedTile.xIndex, clickedTile.yIndex, swapTime);
    }

    bool IsNextTo(Tile start, Tile end)
    {
        if (Mathf.Abs(start.xIndex-end.xIndex) == 1 && start.yIndex==end.yIndex)
        {
            return true;
        }
        if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
        {
            return true;
        }
        return false;
    }

}
