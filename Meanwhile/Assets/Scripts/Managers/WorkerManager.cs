using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : SingletonMonoBehaviour<WorkerManager>
{
    [Header("Workers")]
    [SerializeField] private List<Worker> workers;
    public List<Worker> Workers => workers;
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private Transform recruitPoint;

    [Header("Stations Manager")]
    [SerializeField] private WoodStation[] choppingWoodStations;
    public WoodStation[] ChoppingWoodStations { get { return choppingWoodStations; } }

    [SerializeField] private WorkerHouses[] _workerHouses;
    public WorkerHouses[] workerHouses
    {
        get
        {
            return _workerHouses;
        }
    }

    public GameObject offloadStation;

    private void OnEnable()
    {
        // GameEventBus.Subscribe(GameState.Hire, UpdateWorkerCount);
    }

    void Start()
    {
        choppingWoodStations = FindObjectsOfType<WoodStation>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            WorkerOut();
        }
    }

    public void HireNewWoker()
    {
        if (GameManager.Instance.HasEnoughGold(Modifiers.WorkerPrice) && workers.Count < Modifiers.MaxWorkersCount)
        {
            GameManager.Instance.SpendGold();

            var newHire = Instantiate(workerPrefab, recruitPoint.position, Quaternion.identity);

            workers.Add(newHire.GetComponent<Worker>());

            AssignWorkerStation(newHire.GetComponent<WorkerMovement>());
            AssignWorkerHome(newHire.GetComponent<WorkerMovement>());

            GameEventBus.Publish(GameState.Hire);
        }
        if (workers.Count == 5)
        {
            UIManager.Instance.DisableHireButton();
            print("Limit workers count bud. Cant hire more.");
        }
    }

    // TEMPORARY, MUST BE CLEAND - FOR TESTING PURPOSES
    public void WorkerOut()
    {
        if (workers.Count > 0) { workers[0].Die(); }
        else { print("You don't have more workers to kill bud"); }
    }

    // Look at it as if when the worker dies, the worker manager knows that an empty spot is available. Dark isn't it?
    public void WorkerOut(Worker thisWorker)
    {
        if (workers.Count == 5) { UIManager.Instance.EnableHireButton(); } // THERE SHOULD BE A BETTER WAY TO DO THIS
        workers.Remove(thisWorker);
        GameEventBus.Publish(GameState.Hire); // to update th ui text for the right amount of workers
    }

    public void AllWorkersGoToWork()
    {
        WorkerMovement[] workers = FindObjectsOfType<WorkerMovement>();

        foreach (WorkerMovement worker in workers)
        {
            // worker.ToggleBusy();
            worker.MoveToDestination(worker.WoodChoppingStation);
        }
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

    public void AssignWorkerHome(WorkerMovement worker)
    {
        if (worker.workerHouse == null)
        {
            foreach (WorkerHouses workerHouse in _workerHouses)
            {
                if (!workerHouse.isAssigned)
                {
                    workerHouse.Assign(worker);
                    break;
                }
            }
        }
    }
}
