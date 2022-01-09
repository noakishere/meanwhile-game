using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStation : MonoBehaviour
{
    [SerializeField] private int stationID;
    [SerializeField] public bool isAssigned { get; private set; }

    [SerializeField] private Worker currentWorker;

    private void Start()
    {
        isAssigned = false;
    }

    public void Assign(WorkerMovement newWorker)
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

    public void DeAssign()
    {
        print($"The station {stationID} is no longer assigned to {currentWorker.WorkerName}.");
        isAssigned = false;
        currentWorker = null;
    }
}
