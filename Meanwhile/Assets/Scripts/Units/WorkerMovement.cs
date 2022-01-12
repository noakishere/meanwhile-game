using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerMovement : MonoBehaviour
{
    [Header("NavMesh Agent Config")]
    [SerializeField] private NavMeshAgent agent;
    public float moveSpeed;

    [Header("Destinations")]
    public GameObject testDestination; //testing purposes

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

    [SerializeField] private float agentDistanceModifier; // this is added because agent doesn't get to the point with 0f distance but rather less than 0.5f

    [Header("Worker Config")]

    private Worker workerConfig;
    public Worker WorkerConfig { get { return workerConfig; } }
    [SerializeField] private bool busy;

    private void Awake()
    {
        workerConfig = GetComponent<Worker>();
    }

    void Start()
    {
        AgentSetup();
        agent.SetDestination(transform.position);

        initialPos = transform.position;

        MoveToDestination(woodChoppingStation);
    }

    public bool isStopped;

    // Update is called once per frame
    void Update()
    {
        BehaviourStateMachine();
        AnalyzeState();

        // TESTING PURPOSES
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveToDestination(testDestination);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToDestination(initialPos);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveToDestination(woodChoppingStation);
        }

        transform.position = agent.nextPosition; //fixes the state issue somehow. States update correctly according to the distance. Weird.
    }

    private void AgentSetup()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    // Every frame checks the worker's state and behaves accordingly
    public void BehaviourStateMachine()
    {
        if (workerConfig.CurrentState == WorkerState.Cutting && !busy)
        {
            busy = true;
            StartCoroutine(ChoppingWoodCoroutine());
        }
        else if (workerConfig.CurrentState == WorkerState.Offloading && !busy)
        {
            busy = true;
            StartCoroutine(OffloadingWoodCoroutine());
        }
        else if (workerConfig.CurrentState == WorkerState.Walking)
        {
            // print("Worker is walking");
        }
        else if (workerConfig.CurrentState == WorkerState.Idle)
        {
            // print("Worker is idle");
        }
    }

    // 
    public void AnalyzeState()
    {
        if (woodChoppingStation != null && Vector2.Distance(transform.position, woodChoppingStation.transform.position) <= agentDistanceModifier)
        {
            workerConfig.SetState(WorkerState.Cutting);
        }
        else if (offloadStation != null && Vector3.Distance(transform.position, offloadStation.transform.position) <= 1.1f)
        {
            // print($"<b>{Vector3.Distance(transform.position, offloadStation.transform.position)} for {workerConfig.WorkerName}</b>");
            workerConfig.SetState(WorkerState.Offloading);
        }
        else if (Vector3.Distance(transform.position, agent.destination) > 0)
        {
            workerConfig.SetState(WorkerState.Walking);
        }
        else
        {
            workerConfig.SetState(WorkerState.Idle);
        }
    }

    public IEnumerator ChoppingWoodCoroutine()
    {
        int remainingResourcePlace = workerConfig.resourceCarryCap - workerConfig.carryingWoodAmount;

        for (int i = 0; i < remainingResourcePlace; i++)
        {
            workerConfig.carryingWoodAmount += 1;
            // print($"Cut 1 more wood. {workerConfig.WorkerName} is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        agent.SetDestination(offloadStation.transform.position);
        Debug.Log($"<color=#00FF00><b>{workerConfig.WorkerName} is moving towards {offloadStation.transform.position}</b></color>");
        yield return new WaitForSeconds(1f);
        busy = false;
        yield return null;
    }

    public IEnumerator OffloadingWoodCoroutine()
    {
        int carryingAmount = workerConfig.carryingWoodAmount;

        for (int i = 0; i < carryingAmount; i++)
        {
            workerConfig.carryingWoodAmount -= 1;
            GameManager.Instance.IncrementWood(1);
            // print($"Offloaded 1 more wood. {workerConfig.WorkerName} is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        // TODO: when day's done they should go to rest. A method should decide next move after offloading.
        agent.SetDestination(woodChoppingStation.transform.position);
        Debug.Log($"<color=#00FF00><b>{workerConfig.WorkerName} is moving towards {initialPos}</b></color>");
        yield return new WaitForSeconds(1f);
        busy = false;
        yield return null;
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
}


