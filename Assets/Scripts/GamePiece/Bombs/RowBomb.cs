using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBomb : SpecialGamepiece
{
    public override void OnEnable()
    {
        specialGamepieceType = SpecialGamepieceType.RowBomb;
    }

    public override void PerformBombRule()
    {

    }

}
