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
    [SerializeField] private WorkerMovement workerMovement;
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

    [Header("Worker Animation Section")]
    [SerializeField] private Animator workerAnim;

    void Start()
    {
        workerMovement = GetComponent<WorkerMovement>();
        workerAnim = GetComponent<Animator>();

        isTakeDamage = false;
        GenerateInformation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTakeDamage)
        {
            if (workerHealth <= 0)
            {
                isTakeDamage = false;
                Die();
            }
            else if (currentTimeBetweenDmg >= timeBetweenDmg)
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
        // if (workerHealth <= 0)
        // {
        //     print("HELLO");
        //     isTakeDamage = false; // should fix the constant call
        //     Die();
        // }
        // else
        // {
        currentTimeBetweenDmg = 0f;
        print($"Worker received {dmg} damages");
        // }
        workerHealth -= dmg;
        healthSlider.SetHealth(workerHealth);
    }

    // public void Die()
    // {


    //     workerMovement.Agent.isStopped = true;

    //     SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

    //     LeanTween.value(gameObject, 1f, 0f, 1).setOnUpdate((float val) =>
    //     {
    //         Color c = spriteRenderer.color;
    //         c.a = val;
    //         spriteRenderer.color = c;
    //     }).setOnComplete(() =>
    //     {
    //         WorkerManager.Instance.WorkerOut(this);
    //         workerMovement.workerHouse.GetComponent<WorkerHouses>().DeAssign();
    //         workerMovement.WoodChoppingStation.GetComponent<WoodStation>().DeAssign();
    //         Destroy(this.gameObject, 2f);
    //     });
    // }

    public void Die()
    {
        StartCoroutine(DeathProcess());
    }

    public IEnumerator DeathProcess()
    {
        workerMovement.Agent.isStopped = true;

        workerAnim.SetTrigger("Death");

        yield return new WaitForSeconds(0.3f);

        WorkerManager.Instance.WorkerOut(this);

        workerMovement.workerHouse.GetComponent<WorkerHouses>().DeAssign();
        workerMovement.WoodChoppingStation.GetComponent<WoodStation>().DeAssign();

        Destroy(this.gameObject);
    }

    public void Escape()
    {
        workerMovement.Agent.isStopped = true;

        WorkerManager.Instance.WorkerOut(this, true);

        workerMovement.workerHouse.GetComponent<WorkerHouses>().DeAssign();
        workerMovement.WoodChoppingStation.GetComponent<WoodStation>().DeAssign();

        Destroy(this.gameObject);
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