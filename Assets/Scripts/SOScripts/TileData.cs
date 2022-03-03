using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="TileData",menuName ="Create/Tile Data Holder ")]
public class TileData : ScriptableObject
{
    public TileCreator tilePrefabs;
    public Tile[,] allTiles;
    public int width;
    public int height;


    private void OnEnable()
    {
        allTiles = new Tile[width, height];     
    }

    public void SetupTiles(Board board)
    {
        // We first instantiate obstacle tiles accoring to their specific position
        foreach (var item in tilePrefabs.tiles)
        {
            if (item.tileType==TileType.Obstacle)
            {
                MakeTile(board, item, item.x, item.y,TileType.Obstacle);
            }
        }

        // Lastly we instantiate normal tiles in the empty slots
        foreach (var item in tilePrefabs.tiles)
        {
            if (item.tileType==TileType.Normal)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (allTiles[i,j] == null)
                        {
                            MakeTile(board, item, i, j,TileType.Normal);
                        }
                    }
                }
            }
        }
    }

    private void MakeTile(Board board, TilePrefab item, int i, int j, TileType _tileType)
    {
        GameObject tile = Instantiate(item.prefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
        tile.name = "Tile (" + i + "," + j + ")";
        allTiles[i, j] = tile.GetComponent<Tile>();
        allTiles[i, j].Init(i, j, board, _tileType);
        tile.transform.parent = board.transform;
    }
}
