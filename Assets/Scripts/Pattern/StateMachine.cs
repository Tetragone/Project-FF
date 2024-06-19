using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : MonoBehaviour where T : Enum
{ 
    public T NowState;
    public T NextState;
    public Dictionary<T, int> StatePriority = new Dictionary<T, int>();

    public void ChangeState(T nextState)
    {
        if (NextState != null && StatePriority[NextState] < StatePriority[nextState])
        {
            NextState = nextState;
        }
    }

    private void Update()
    {
        
    }
}
