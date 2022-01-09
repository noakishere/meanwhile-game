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



    void Start()
    {
        GenerateInformation();
    }

    private void GenerateInformation()
    {
        var namesArray = namesList.names;
        workerName = namesArray[Random.Range(0, namesArray.Length)];
        textMesh.text = workerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // TakeDamage(Modifiers.WorkerDamageSmall);
        }

        if (state == WorkerState.Walking)
        {
            if (currentTimeBetweenDmg >= timeBetweenDmg)
            {
                TakeDamage(Modifiers.WorkerDamageSmall);
            }
            currentTimeBetweenDmg += timeBetweenDmgModifier * Time.deltaTime;
        }

    }

    public void TakeDamage(float dmg)
    {
        workerHealth -= dmg;
        currentTimeBetweenDmg = 0f;
        print($"Worker received {dmg} damages");
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



