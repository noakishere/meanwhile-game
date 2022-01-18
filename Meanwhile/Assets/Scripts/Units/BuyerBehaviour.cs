using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BuyerBehaviour : MonoBehaviour
{
    [Header("Buyer timing section")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    [SerializeField] private float timeUntilSpawn;

    [SerializeField] private float currentTimeBetween;

    [SerializeField] private float waitingTimeModifier;

    [Header("Ship section")]
    [SerializeField] private GameObject buyerShip;
    [SerializeField] private Transform shipDockSection;

    [SerializeField] private Vector2 initialPos;

    [SerializeField] private bool isShipHere;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        // GameEventBus.Unsubscribe(GameState.Buyer, Helpers.ControlTime);
    }

    private void Start()
    {
        initialPos = buyerShip.transform.position;

        timeUntilSpawn = ReturnRandNumber();

        isShipHere = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(BuyerArrives());
        }

        if (currentTimeBetween >= timeUntilSpawn)
        {
            isShipHere = true;
            StartCoroutine(BuyerArrives());
        }

        else if (!isShipHere)
        {
            currentTimeBetween += waitingTimeModifier * Time.deltaTime;
        }

    }

    private float ReturnRandNumber()
    {
        return Random.Range(minTime, maxTime);
    }

    private void MoveTheShipToDock()
    {
        LeanTween.move(buyerShip, shipDockSection, 2f);
    }

    public void MoveTheShipBack()
    {
        print($"Ship going back to {initialPos}");
        LeanTween.move(buyerShip, initialPos, 2f);
        isShipHere = false;
    }

    public IEnumerator BuyerArrives()
    {
        currentTimeBetween = 0f;
        MoveTheShipToDock();

        yield return new WaitForSeconds(2.1f);

        UIManager.Instance.ToggleBuyerPanel();
        yield return null;
    }


}
