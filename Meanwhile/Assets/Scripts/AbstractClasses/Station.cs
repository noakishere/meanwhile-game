using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Station : MonoBehaviour
{
    [SerializeField] protected int stationID;

    public bool isAssigned;

    private void Start()
    {
        stationID = GetInstanceID();
        isAssigned = false;
    }

    public abstract void Assign(WorkerMovement newWorker);
    public abstract void DeAssign();

}
