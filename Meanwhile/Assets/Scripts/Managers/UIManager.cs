using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public TMP_Text woodsText;

    public GameObject pauseMenu;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.Pause, ShowPauseMenu);
        GameEventBus.Subscribe(GameState.Normal, HidePauseMenu);
    }


    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.Pause, ShowPauseMenu);
    }

    void Start()
    {

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

    public void UpdateWoodsText(int woods)
    {
        woodsText.text = $"Woods: {woods}";
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0.02f;
        pauseMenu.SetActive(true);
    }

    private void HidePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

}
