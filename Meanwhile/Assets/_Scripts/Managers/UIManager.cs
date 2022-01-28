using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [Header("Resources")]
    [SerializeField] private TMP_Text woodsText;
    [SerializeField] private TMP_Text goldsText;
    [SerializeField] private TMP_Text workersText;
    [SerializeField] private TMP_Text dayCountText;

    [Header("GUI Elements")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject buyerPanel;

    [Header("Buttons")]
    [SerializeField] private Button hireButton;
    [SerializeField] private Button buyerPanelSellButton;
    [SerializeField] private TMP_Text buyerPanelSellText;

    private void OnEnable()
    {
        /*
        *
        * Resource section
        *
        */
        GameEventBus.Subscribe(GameState.GoldUpdate, UpdateGoldsText);
        GameEventBus.Subscribe(GameState.WoodUpdate, UpdateWoodsText);
        GameEventBus.Subscribe(GameState.Hire, UpdateWorkersText);

        /*
        *
        * Menu Section
        *
        */
        GameEventBus.Subscribe(GameState.Pause, ShowPauseMenu);
        GameEventBus.Subscribe(GameState.Normal, HidePauseMenu);

        GameEventBus.Subscribe(GameState.Sell, UpdateSellText);
    }



    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.Pause, ShowPauseMenu);
    }

    void Start()
    {
        UpdateWoodsText();
        UpdateGoldsText();
        UpdateWorkersText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                GameEventBus.Publish(GameState.Normal);
            }
            else
            {
                GameEventBus.Publish(GameState.Pause);
            }
        }

        UpdateWorkersText();
    }

    public void UpdateWoodsText()
    {
        woodsText.text = $"{GameManager.Instance.Woods}";
    }

    public void UpdateGoldsText()
    {
        goldsText.text = $"{GameManager.Instance.Golds}";
    }

    public void UpdateWorkersText()
    {
        workersText.text = $"{WorkerManager.Instance.Workers.Count}/5";
        if (GameManager.Instance.Golds < Modifiers.WorkerPrice || WorkerManager.Instance.Workers.Count == Modifiers.WorkerCap)
            DisableHireButton();
        else
            EnableHireButton();
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    private void HidePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void ToggleBuyerPanel()
    {
        UpdateSellText();
        buyerPanel.SetActive(!buyerPanel.activeInHierarchy);
    }

    public void UpdateSellText()
    {
        buyerPanelSellText.text = GameManager.Instance.Woods > 5 ? "Sell 5 woods" : GameManager.Instance.Woods > 0 ? $"Sell {GameManager.Instance.Woods}" : "No woods available to sell";
        if (GameManager.Instance.Woods <= 0)
        {
            buyerPanelSellButton.enabled = false;
        }
        else
        {
            buyerPanelSellButton.enabled = true;
        }
    }

    public void UpdateDayCountText()
    {
        dayCountText.text = $"Day: {DayManager.Instance.DayCount}";
    }

    public void DisableHireButton()
    {
        hireButton.enabled = false;
        hireButton.GetComponentInChildren<TMP_Text>().text = "no more";
    }

    public void EnableHireButton()
    {
        hireButton.enabled = true;
        hireButton.GetComponentInChildren<TMP_Text>().text = "Hire";
    }

}
