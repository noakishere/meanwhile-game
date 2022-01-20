using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerOffloadingState : MonoBehaviour, IWorkerState
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
            StartCoroutine(OffloadingWoodCoroutine());
        }
    }

    public IEnumerator OffloadingWoodCoroutine()
    {
        int carryingAmount = _workerMovement.WorkerConfig.carryingWoodAmount;

        for (int i = 0; i < carryingAmount; i++)
        {
            _workerMovement.WorkerConfig.carryingWoodAmount -= 1;
            GameManager.Instance.IncrementWood(1);
            // print($"Offloaded 1 more wood. {workerConfig.WorkerName} is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        // TODO: when day's done they should go to rest. A method should decide next move after offloading.
        // _workerMovement.Agent.SetDestination(_workerMovement.WoodChoppingStation.transform.position);
        _workerMovement.WhereToGoNext(_workerMovement.WoodChoppingStation);
        Debug.Log($"<color=#00FF00><b>{_workerMovement.WorkerConfig.WorkerName} is moving towards {_workerMovement.WoodChoppingStation}</b></color>");
        yield return new WaitForSeconds(1f);
        _workerMovement.ToggleBusy();
        yield return null;
    }
}
