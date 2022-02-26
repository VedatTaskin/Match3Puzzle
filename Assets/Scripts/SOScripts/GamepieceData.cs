using UnityEngine;


[CreateAssetMenu(fileName ="Gamepieces Data", menuName ="Create/Gamepiece Data Holder")]
public class GamepieceData : ScriptableObject
{
    public Gamepiece[,] allGamepieces;
    public GameObject[] gamePiecePrefabs;
    public TileData tileData;

    private void OnEnable()
    {
        allGamepieces = new Gamepiece[tileData.width, tileData.height];
    }

    public GameObject GetRandomGamePiece()
    {
        int randomIndex = Random.Range(0, gamePiecePrefabs.Length);
        if (gamePiecePrefabs[randomIndex] == null)
        {
            Debug.LogWarning("BOARD" + randomIndex + " doesn't contain a vaild Gamepiece prefab");
        }
        return gamePiecePrefabs[randomIndex];
    }

}
