using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : Gamepiece
{
    public override GamepieceType gamepieceType => GamepieceType.Collectible;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;


}
