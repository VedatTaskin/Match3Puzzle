using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="TileData",menuName ="Create/Tile Data Holder ")]
public class TileData : ScriptableObject
{
    public GameObject tilePrefab;
    public Tile[,] allTiles;
    public int width;
    public int height;


    private void OnEnable()
    {
        allTiles = new Tile[width, height];     
    }


    public void SetupTiles(Board board)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                tile.name = "Tile (" + i + "," + j + ")";
                allTiles[i, j] = tile.GetComponent<Tile>();
                allTiles[i, j].Init(i, j, board);
                tile.transform.parent = board.transform;
            }

        }
    }

}
