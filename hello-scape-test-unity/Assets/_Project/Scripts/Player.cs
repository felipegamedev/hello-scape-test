using System;
using UnityEngine;

public class Player : MovableRoomObject
{
    public override void Initialize(Vector2 p_floorSlotSize)
    {
        base.Initialize(p_floorSlotSize);
        IsPlayer = true;
    }
}
