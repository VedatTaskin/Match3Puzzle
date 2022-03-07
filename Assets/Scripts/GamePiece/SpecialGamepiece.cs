using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGamepiece : Gamepiece, ISpecialGamepieceRule
{
    public override GamepieceType gamepieceType => GamepieceType.Special;

    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;
    public virtual List<Gamepiece> PerformRule(Gamepiece gamepiece)
    {
        return null;
    }
}
