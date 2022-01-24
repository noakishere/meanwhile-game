using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerIdleState : MonoBehaviour, IWorkerState
{
    private WorkerMovement _workerMovement;

    public void Handle(WorkerMovement workerMovement)
    {
        if (!_workerMovement) { _workerMovement = workerMovement; }

        // _workerMovement.ToggleBusy();
    }
}
