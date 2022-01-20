using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHomeState : MonoBehaviour, IWorkerState
{
    private WorkerMovement _workerMovement;

    public void Handle(WorkerMovement workerMovement)
    {
        if (!_workerMovement) { _workerMovement = workerMovement; }

        _workerMovement.isHome = true;
        if (_workerMovement.isBusy) _workerMovement.ToggleBusy();
        _workerMovement.WorkerConfig.isTakeDamage = false;
    }
}
