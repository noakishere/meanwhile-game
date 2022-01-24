using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerWalkingState : MonoBehaviour, IWorkerState
{
    private WorkerMovement _workerMovement;
    public void Handle(WorkerMovement workerMovement)
    {
        if (!_workerMovement) { _workerMovement = workerMovement; }
        _workerMovement.isHome = false;
        _workerMovement.WorkerConfig.isTakeDamage = true;
    }
}
