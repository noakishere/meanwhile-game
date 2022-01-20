using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    [SerializeField] private int workerHealth;
    public int WorkerHealth
    {
        get { return workerHealth; }
    }
    [SerializeField] private float timeBetweenDmg;
    [SerializeField] private float timeBetweenDmgModifier;

    [SerializeField] private float currentTimeBetweenDmg;

    [Header("Information")]
    public WorkerMovement workerMovement;
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

    [SerializeField] private WorkerHealthBarUIScript healthSlider;

    public bool isTakeDamage;
    void Start()
    {
        workerMovement = GetComponent<WorkerMovement>();
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
        else if (workerMovement.isHome)
        {

        }
    }

    private void GenerateInformation()
    {
        var namesArray = namesList.names;
        workerName = namesArray[Random.Range(0, namesArray.Length)];
        textMesh.text = workerName;
    }

    public void TakeDamage(int dmg)
    {
        workerHealth -= dmg;
        healthSlider.SetHealth(workerHealth);
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