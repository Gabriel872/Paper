using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;

    void Update()
    {

        RunFiniteStateMachine();
    }

    private void RunFiniteStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchState(nextState);
        }
    }

    private void SwitchState(State nextState)
    {
        currentState = nextState;
    }
}
