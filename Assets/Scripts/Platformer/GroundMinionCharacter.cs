using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMinionCharacter : GroundCharacter2D
{
    /// <summary>
    /// to enable/disable the faced direction based on movemment
    /// </summary>
    public bool bManualFaceDirection = false;

    public void Move(Vector2 moveDir, bool jump = false, int manualDirection = 1)
    {
        base.Move(moveDir, jump);

        if (!bManualFaceDirection)
            DirectionLogic((int)moveVector.x);
        else
            DirectionLogic(manualDirection);
    }
}
