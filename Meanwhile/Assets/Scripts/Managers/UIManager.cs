using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // THESE NEED TO BE CLEANED
    public TMP_Text woodsText;
    public TMP_Text goldsText;
    public TMP_Text workersText;

    public GameObject pauseMenu;

    public Button hireButton;
    [SerializeField] private GameObject buyerPanel;
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
    }

    public void UpdateWoodsText()
    {
        woodsText.text = $"Woods: {GameManager.Instance.Woods}";
    }

    public void UpdateGoldsText()
    {
        goldsText.text = $"Golds: {GameManager.Instance.Golds}";
    }

    public void UpdateWorkersText()
    {
        workersText.text = $"Workers: {WorkerManager.Instance.Workers.Count}/5";
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
