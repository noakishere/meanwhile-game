using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MonoBehaviour
{
    #region Properties
    [Header("NavMesh Agent Config")]
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    [SerializeField] private float moveSpeed;

    private void AgentSetup()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    [Header("Destinations")]
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private GameObject woodChoppingStation;
    public GameObject WoodChoppingStation
    {
        get
        {
            return woodChoppingStation;
        }
    }
    public GameObject offloadStation;

    [SerializeField] public GameObject workerHouse;

    [SerializeField] private float agentDistanceModifier; // this is added because agent doesn't get to the point with 0f distance but rather less than 0.5f



    [Header("Worker Config")]
    private Worker workerConfig;
    public Worker WorkerConfig { get { return workerConfig; } }
    [SerializeField] private bool busy;
    public bool isBusy => busy;

    public bool isHome;

    private void Awake()
    {
        workerConfig = GetComponent<Worker>();
    }

    // Worker State Management
    private IWorkerState _idleState, _walkingState, _cuttingState, _offloadingState, _workerHomeState;

    private WorkerStateContext _workerStateContext;

    private void StateContextSetup()
    {
        _workerStateContext = new WorkerStateContext(this);

        _idleState = gameObject.AddComponent<WorkerIdleState>();
        _walkingState = gameObject.AddComponent<WorkerWalkingState>();
        _cuttingState = gameObject.AddComponent<WorkerCuttingState>();
        _offloadingState = gameObject.AddComponent<WorkerOffloadingState>();
        _workerHomeState = gameObject.AddComponent<WorkerHomeState>();

        _workerStateContext.Transition(_idleState);
    }

    #endregion

    #region UnityMethods
    void Start()
    {
        busy = false;
        AgentSetup();
        StateContextSetup();

        initialPos = transform.position;

        MoveToDestination(woodChoppingStation);
    }

    void Update()
    {
        AnalyzeState();

        transform.position = agent.nextPosition; //fixes the state issue somehow. States update correctly according to the distance. Weird.
    }

    #endregion

    #region Methods
    public void AnalyzeState()
    {
        if (woodChoppingStation != null && Vector2.Distance(transform.position, woodChoppingStation.transform.position) <= agentDistanceModifier)
        {
            _workerStateContext.Transition(_cuttingState);
        }
        else if (offloadStation != null && Vector3.Distance(transform.position, offloadStation.transform.position) <= 1.1f)
        {
            _workerStateContext.Transition(_offloadingState);
        }
        else if (Vector3.Distance(transform.position, agent.destination) > 0)
        {
            _workerStateContext.Transition(_walkingState);
        }
        else if (Vector3.Distance(transform.position, workerHouse.transform.position) <= agentDistanceModifier)
        {
            _workerStateContext.Transition(_workerHomeState);
        }
        else
        {
            _workerStateContext.Transition(_idleState);
        }
    }

    public void MoveToDestination(Vector3 newDestination)
    {
        if (!busy)
        {
            Debug.Log($"<color=#00FF00><b>{workerConfig.WorkerName} is moving towards {newDestination}</b></color>");
            agent.SetDestination(newDestination);
        }
        else
        {
            print($"<b><color=red>Currently busy because {workerConfig.CurrentState}, can't Move to {newDestination}</color></b>");
        }
    }

    public void MoveToDestination(GameObject newDestination)
    {
        if (!busy)
        {
            Debug.Log($"<color=#00FF00><b>{workerConfig.WorkerName} is moving towards {newDestination.name}</b></color>");
            agent.SetDestination(newDestination.transform.position);
        }
        else
        {
            print($"<b><color=red>Currently busy {workerConfig.CurrentState}, can't Move to {newDestination.name}</color></b>");
        }
    }

    public void AssignWoodStation(WoodStation ws)
    {
        woodChoppingStation = ws.gameObject;
    }

    public void ToggleBusy()
    {
        busy = !busy;
    }

    public void WhereToGoNext(GameObject nextDestination)
    {
        if (DayManager.Instance.IsDayDone)
        {
            busy = false;
            agent.SetDestination(workerHouse.transform.position);
        }
        else
        {
            agent.SetDestination(nextDestination.transform.position);
        }
    }

    #endregion
}


