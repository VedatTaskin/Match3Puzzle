using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : Gamepiece, IGamepieceRule,ISelfDestroy
{
    public override GamepieceType gamepieceType => GamepieceType.Bomb;
    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;
    public virtual List<Gamepiece> PerformRule(Gamepiece gamepiece,Board board, Gamepiece otherGamepiece=null)
    {
        return null;
    }
    public virtual List<Gamepiece> SelfDestroy(Board board)
    {
        return null;
    }
}
