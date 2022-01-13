using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private WorkerState state;
    public WorkerState CurrentState
    {
        get
        {
            return state;
        }
    }

    [Header("Health related")]
    [SerializeField] private float workerHealth;
    public float WorkerHealth
    {
        get { return workerHealth; }
    }
    [SerializeField] private float timeBetweenDmg;
    [SerializeField] private float timeBetweenDmgModifier;

    [SerializeField] private float currentTimeBetweenDmg;

    [Header("Information")]
    [SerializeField] private NamesBank namesList;
    [SerializeField] private string workerName;
    public string WorkerName
    {
        get
        {
            return workerName;
        }
    }

    [Header("Working information")]
    public int resourceCarryCap;
    public int carryingWoodAmount;

    [Header("UI manage")]
    [SerializeField] private GameObject textMeshContainer;
    public TextMesh textMesh;

    public bool isTakeDamage;
    void Start()
    {
        isTakeDamage = false;
        GenerateInformation();
    }

    // Update is called once per frame
    void Update()
    {
        // if (state == WorkerState.Walking) // Maybe if != idle then take damage? we'll see
        if (isTakeDamage)
        {
            if (currentTimeBetweenDmg >= timeBetweenDmg)
            {
                TakeDamage(Modifiers.WorkerDamageSmall);
            }
            currentTimeBetweenDmg += timeBetweenDmgModifier * Time.deltaTime;
        }
    }

    private void GenerateInformation()
    {
        var namesArray = namesList.names;
        workerName = namesArray[Random.Range(0, namesArray.Length)];
        textMesh.text = workerName;
    }

    public void TakeDamage(float dmg)
    {
        workerHealth -= dmg;
        if (workerHealth <= 0)
        {
            Die();
        }
        else
        {
            currentTimeBetweenDmg = 0f;
            print($"Worker received {dmg} damages");
        }
    }

    public void Die()
    {
        WorkerManager.Instance.WorkerOut(this);
        gameObject.GetComponent<WorkerMovement>().workerHouse.GetComponent<WorkerHouses>().DeAssign();
        gameObject.GetComponent<WorkerMovement>().WoodChoppingStation.GetComponent<WoodStation>().DeAssign();
        Destroy(gameObject);
    }


    public void SetState(WorkerState newState)
    {
        state = newState;
    }

    private void OnMouseEnter()
    {
        textMeshContainer.SetActive(true);
    }

    private void OnMouseExit()
    {
        textMeshContainer.SetActive(false);
    }
}