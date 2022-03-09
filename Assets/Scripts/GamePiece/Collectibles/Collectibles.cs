using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : Gamepiece, ICollectibleRule
{
    public override GamepieceType gamepieceType => GamepieceType.Collectible;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;

    public void CollectibleRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece)
    {

    }


}
