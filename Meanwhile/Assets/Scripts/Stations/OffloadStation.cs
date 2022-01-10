using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffloadStation : Station
{
    public Queue<WorkerMovement> workersLine;

    public void GetInLine(WorkerMovement newWorker)
    {
        workersLine.Enqueue(newWorker);
    }

    public void GetOutOfLine()
    {
        workersLine.Dequeue();
    }


    public override void Assign(WorkerMovement newWorker)
    {
        if (isAssigned)
        {
            print($"{workersLine.Dequeue()} is currently offloading. {newWorker.WorkerConfig.WorkerName} get in line.");
            GetInLine(newWorker);
        }
        else
        {

        }
    }

    public override void DeAssign()
    {
        GetOutOfLine();
    }
}
