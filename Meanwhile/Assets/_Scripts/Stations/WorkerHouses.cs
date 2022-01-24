using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHouses : Station
{
    [SerializeField] private Worker currentWorker;

    public override void Assign(WorkerMovement newWorker)
    {
        if (isAssigned)
        {
            print($"The house {this.gameObject.name} is already assigned to {currentWorker.WorkerName}.");
        }
        else
        {
            currentWorker = newWorker.WorkerConfig; //WorkerConfig returns Worker class
            newWorker.workerHouse = this.gameObject;
            print($"The house {this.gameObject.name} is being assigned to {currentWorker.WorkerName}.");
            isAssigned = true;
        }
    }

    public override void DeAssign()
    {
        isAssigned = false;
        currentWorker = null;
    }
}
