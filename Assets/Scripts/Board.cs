using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public int borderSize;

    public GameObject tilePrefab;
    public GameObject[] gamePiecePrefabs;

    Tile[,] m_AllTiles;
    GamepieceBase[,] m_AllGamePieces;

    private void Start()
    {
        m_AllTiles = new Tile[width, height];
        m_AllGamePieces = new GamepieceBase[width, height];

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

    void PlaceGamePiece(GamepieceBase gamePiece,int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD: invalid gamepiece");
            return;
        }
        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;
        gamePiece.SetCoordinate(x, y);
    }

    void FillRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;
                randomPiece.transform.parent = transform;

                if (randomPiece !=null)
                {
                    PlaceGamePiece(randomPiece.GetComponent<GamepieceBase>(), i, j);
                }

            }

        }
    }

}
