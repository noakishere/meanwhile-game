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
    public GameObject testDestination;

    Vector3 initialPos;
    public GameObject woodChoppingStation;
    public GameObject offloadStation;

    [SerializeField] private float agentDistanceModifier; // this is added because agent doesn't get to the point with 0f distance but rather less than 0.5f

    [Header("Worker Config")]

    private Worker workerConfig;
    [SerializeField] private bool busy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        agent.SetDestination(transform.position);

        initialPos = transform.position;

        workerConfig = GetComponent<Worker>();
    }

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
    }

    public void BehaviourStateMachine()
    {
        if (workerConfig.CurrentState == WorkerState.Cutting && !busy)
        {
            // IF already CAP move to offload -> TODO
            busy = true;
            StartCoroutine(ChoppingWoodCoroutine());
        }
        else if (workerConfig.CurrentState == WorkerState.Offloading && !busy)
        {
            // IF already empty move to wood chopping station -> TODO TODO
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

    public void AnalyzeState()
    {
        if (Vector2.Distance(transform.position, woodChoppingStation.transform.position) <= agentDistanceModifier)
        {
            workerConfig.SetState(WorkerState.Cutting);
        }
        else if (Vector3.Distance(transform.position, offloadStation.transform.position) <= agentDistanceModifier)
        {
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
        for (int i = 0; i < workerConfig.resourceCarryCap; i++)
        {
            workerConfig.carryingWoodAmount += 1;
            print($"Cut 1 more wood. Worker is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        agent.SetDestination(offloadStation.transform.position);
        Debug.Log($"<color=#00FF00><b>Moving towards {offloadStation.transform.position}</b></color>");
        yield return new WaitForSeconds(1f);
        busy = false;
        yield return null;
    }

    public IEnumerator OffloadingWoodCoroutine()
    {
        var carryingAmount = workerConfig.carryingWoodAmount;

        for (int i = 0; i < carryingAmount; i++)
        {
            workerConfig.carryingWoodAmount -= 1;
            GameManager.Instance.woods += 1;
            print($"Cut 1 more wood. Worker is currently carrying {workerConfig.carryingWoodAmount}");
            yield return new WaitForSeconds(2f);
        }

        agent.SetDestination(initialPos);
        Debug.Log($"<color=#00FF00><b>Moving towards {initialPos}</b></color>");
        yield return new WaitForSeconds(1f);
        busy = false;
        yield return null;
    }

    public void MoveToDestination(Vector3 newDestination)
    {
        if (!busy)
        {
            Debug.Log($"<color=#00FF00><b>Moving towards {newDestination}</b></color>");
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
            Debug.Log($"<color=#00FF00><b>Moving towards {newDestination.name}</b></color>");
            agent.SetDestination(newDestination.transform.position);
        }
        else
        {
            print($"<b><color=red>Currently busy {workerConfig.CurrentState}, can't Move to {newDestination.name}</color></b>");
        }
    }
}


