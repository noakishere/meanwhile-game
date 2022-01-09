using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : SingletonMonoBehaviour<WorkerManager>
{
    [SerializeField] private WoodStation[] choppingWoodStations;
    public WoodStation[] ChoppingWoodStations { get { return choppingWoodStations; } }


    // Start is called before the first frame update
    void Start()
    {
        choppingWoodStations = FindObjectsOfType<WoodStation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var workers = FindObjectsOfType<WorkerMovement>();
            foreach (WorkerMovement worker in workers)
            {
                AssignWorkerStation(worker);
            }
        }
    }

    public void AssignWorkerStation(WorkerMovement worker)
    {
        foreach (WoodStation station in choppingWoodStations)
        {
            if (!station.isAssigned)
            {
                station.Assign(worker);
                break;
            }
        }
    }
}
