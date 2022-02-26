using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName ="Gamepieces Data", menuName ="Create/All Gamepiece Data Holder")]
public class GamepieceData : ScriptableObject
{
    Gamepiece[,] allGamepieces;
}
