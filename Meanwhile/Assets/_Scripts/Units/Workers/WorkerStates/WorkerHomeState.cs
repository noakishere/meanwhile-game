using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHomeState : MonoBehaviour, IWorkerState
{
    private WorkerMovement _workerMovement;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.DayStart, AtHomeBehaviour);
    }

    private void OnDisable()
    {
        GameEventBus.Subscribe(GameState.DayStart, AtHomeBehaviour);
    }

    public void Handle(WorkerMovement workerMovement)
    {
        if (!_workerMovement) { _workerMovement = workerMovement; }

        if (!_workerMovement.isHome)
        {
            if (_workerMovement.isBusy) _workerMovement.ToggleBusy();
            _workerMovement.WorkerConfig.isTakeDamage = false;
        }
        _workerMovement.isHome = true;
        _workerMovement.isGoingHome = false;
    }

    public void AtHomeBehaviour()
    {
        if (_workerMovement.WorkerConfig.WorkerHealth < 40)
        {
            int randNum = Random.Range(0, 10);
            if (randNum <= 4)
            {
                _workerMovement.WorkerConfig.Escape();
            }
        }
    }
}
