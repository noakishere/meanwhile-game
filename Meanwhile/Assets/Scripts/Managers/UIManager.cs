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
        Time.timeScale = 0.1f;
        pauseMenu.SetActive(true);
    }

    private void HidePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
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
