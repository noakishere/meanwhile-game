using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCuttingState : MonoBehaviour, IWorkerState
{
    private WorkerMovement _workerMovement;

    public void Handle(WorkerMovement workerMovement)
    {
        if (!_workerMovement) { _workerMovement = workerMovement; }

        if (!_workerMovement.isBusy)
        {
            _workerMovement.isHome = false;
            _workerMovement.WorkerConfig.isTakeDamage = false;
            _workerMovement.ToggleBusy();
            StartCoroutine(ChoppingWoodCoroutine());
        }
    }

    public IEnumerator ChoppingWoodCoroutine()
    {
        int remainingResourcePlace = _workerMovement.WorkerConfig.resourceCarryCap - _workerMovement.WorkerConfig.carryingWoodAmount;

        for (int i = 0; i < remainingResourcePlace; i++)
        {
            _workerMovement.WorkerConfig.carryingWoodAmount += 1;
            // print($"Cut 1 more wood. {workerConfig.WorkerName} is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        // _workerMovement.Agent.SetDestination(_workerMovement.offloadStation.transform.position);
        _workerMovement.WhereToGoNext(_workerMovement.offloadStation);
        Debug.Log($"<color=#00FF00><b>{_workerMovement.WorkerConfig.WorkerName} is moving towards {_workerMovement.Agent.destination}</b></color>");
        yield return new WaitForSeconds(1f);
        _workerMovement.ToggleBusy();
        yield return null;
    }
}
