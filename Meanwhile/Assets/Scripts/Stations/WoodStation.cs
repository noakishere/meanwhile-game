using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStation : Station
{
    [SerializeField] private Worker currentWorker;

    public override void Assign(WorkerMovement newWorker)
    {
        if (isAssigned)
        {
            print($"The station {stationID} is already assigned to {currentWorker.WorkerName}.");
        }
        else
        {
            currentWorker = newWorker.WorkerConfig; //WorkerConfig returns Worker class
            newWorker.AssignWoodStation(this);
            print($"The station {stationID} is being assigned to {currentWorker.WorkerName}.");
            isAssigned = true;
        }
    }

    public override void DeAssign()
    {
        // print($"The station {stationID} is no longer assigned to {currentWorker.WorkerName}.");
        isAssigned = false;
        currentWorker = null;
    }
}
