using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tile", menuName = "Create / New tile")]
public class TileCreator : ScriptableObject
{
    public List<TilePrefab> tiles = new List<TilePrefab>();

}

[System.Serializable]
public class TilePrefab
{
    public GameObject prefab;
    public TileType tileType;
    public int x;
    public int y;
    public int z;
}
