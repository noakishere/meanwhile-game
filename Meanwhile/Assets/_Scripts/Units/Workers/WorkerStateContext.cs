using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerStateContext
{
    public IWorkerState CurrentState
    {
        get; set;
    }

    private readonly WorkerMovement _workerMovement;

    public WorkerStateContext(WorkerMovement workerMovement)
    {
        _workerMovement = workerMovement;
    }

    public void Transition()
    {
        CurrentState.Handle(_workerMovement);
    }

    public void Transition(IWorkerState state)
    {
        CurrentState = state;
        CurrentState.Handle(_workerMovement);
    }
}
