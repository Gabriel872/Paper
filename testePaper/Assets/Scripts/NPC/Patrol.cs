using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State
{
    public string chaseColor;
    public Chase chaseState;
    public Guard guard;
    public override State RunCurrentState()
    {
        if (chaseColor.Equals(PlayerMove.intance.currentColorName))
        {
            guard.PatrolControl(false);
            return chaseState;
        }
        else
        {
            guard.PatrolControl(true);
            return this;
        }
    }
}
