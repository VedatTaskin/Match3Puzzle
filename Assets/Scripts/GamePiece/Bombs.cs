using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : Gamepiece, IBombRule
{
    public override GamepieceType gamepieceType => GamepieceType.Bomb;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;
    public virtual List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return null;
    }
}
