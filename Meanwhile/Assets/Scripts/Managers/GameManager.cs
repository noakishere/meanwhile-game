using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("Resources")]
    [SerializeField] private int woods;
    public int Woods => woods;
    [SerializeField] private int golds;
    public int Golds => golds;

    private void OnEnable()
    {

    }

    private void Update()
    {
        // TESTING stuff - To be cleaned
        if (Input.GetKeyDown(KeyCode.K))
        {
            IncrementWood(5);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            IncrementGold(5);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpendGold(5);
        }
    }

    public void IncrementWood(int woodNum)
    {
        woods += woodNum;
        GameEventBus.Publish(GameState.WoodUpdate);
    }
    public void IncrementGold(int goldNum)
    {
        golds += goldNum;
        GameEventBus.Publish(GameState.GoldUpdate);
    }

    public void SpendGold(int goldToSpend)
    {
        if (golds - goldToSpend >= 0)
        {
            golds -= goldToSpend;
            GameEventBus.Publish(GameState.GoldUpdate);
        }
        else
        {
            // TODO: proper display that gold isn't enough to do this.
            print($"Not enough gold is available. {goldToSpend} is out of our {golds} budget.");
        }
    }
}