using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("Resources")]
    public int woods;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.Pause, delegate { print("Pause babes"); });
    }

    private void Update()
    {

    }

    public void IncrementWood(int woodNum)
    {
        woods += woodNum;
        UIManager.Instance.UpdateWoodsText(woods);
    }

}
