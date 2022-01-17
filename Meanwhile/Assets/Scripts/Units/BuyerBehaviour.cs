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

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.Buyer, Helpers.ControlTime);
    }

    private void OnDisable()
    {
        // GameEventBus.Unsubscribe(GameState.Buyer, Helpers.ControlTime);
    }

    private void Start()
    {
        timeUntilSpawn = ReturnRandNumber();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(BuyerArrives());
        }

        if (currentTimeBetween >= timeUntilSpawn)
        {
            StartCoroutine(BuyerArrives());
            // MoveTheShip();
            // GameEventBus.Publish(GameState.Buyer);
            // currentTimeBetween = 0f;
        }

        currentTimeBetween += waitingTimeModifier * Time.deltaTime;
    }

    private float ReturnRandNumber()
    {
        return Random.Range(minTime, maxTime);
    }

    private void MoveTheShip()
    {
        LeanTween.move(buyerShip, shipDockSection, 2f);
    }

    public IEnumerator BuyerArrives()
    {
        MoveTheShip();

        yield return new WaitForSeconds(2.1f);

        GameEventBus.Publish(GameState.Buyer);
        currentTimeBetween = 0f;
        yield return null;
    }
}
