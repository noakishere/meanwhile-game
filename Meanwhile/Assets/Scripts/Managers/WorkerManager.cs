using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : SingletonMonoBehaviour<WorkerManager>
{
    [Header("Workers")]
    [SerializeField] private List<Worker> workers;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform recruitPoint;

    [Header("Stations Manager")]
    [SerializeField] private WoodStation[] choppingWoodStations;
    public WoodStation[] ChoppingWoodStations { get { return choppingWoodStations; } }
    public GameObject offloadStation;



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
                worker.offloadStation = offloadStation;
                AssignWorkerStation(worker);
            }
        }
    }

    public void HireNewWoker()
    {
        var newHire = Instantiate(workerPrefab, recruitPoint.position, Quaternion.identity);
        workers.Add(newHire.GetComponent<Worker>());
        AssignWorkerStation(newHire.GetComponent<WorkerMovement>());
    }

    public void WorkerOut(Worker thisWorker)
    {
        workers.Remove(thisWorker);
    }

    public void AssignWorkerStation(WorkerMovement worker)
    {
        if (worker.WoodChoppingStation == null)
        {
            worker.offloadStation = offloadStation;
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
}
