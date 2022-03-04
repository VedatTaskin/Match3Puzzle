using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGamepiece : Gamepiece,ISpecialGamepieceRule
{



    public virtual void PerformBombRule()
    {
        Debug.Log(this.gamepieceType + " my type");
        Debug.Log(this.specialGamepieceType + " my special type");
    }
}
