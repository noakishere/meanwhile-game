using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : SingletonMonoBehaviour<DayManager>
{
    [Header("Modifiers")]
    [SerializeField] private bool isDayDone;
    public bool IsDayDone => isDayDone;

    [SerializeField] private bool isCount;
    [SerializeField] private int dayCount;
    public int DayCount => dayCount;

    [SerializeField] private float timeCount;
    [SerializeField] private float endDayCount;
    [SerializeField] private float timeIncreaseModifier;

    [Header("UI Section")]
    [SerializeField] private GameObject dayNightOverlay;
    [SerializeField] private Image dayNightOverlayColor;
    [SerializeField] private GameObject continueDayButton;


    void Start()
    {
        dayNightOverlay.SetActive(false);
        dayNightOverlayColor = dayNightOverlay.GetComponent<Image>();

        dayCount = 1;
        UIManager.Instance.UpdateDayCountText();

        isCount = true;
    }

    void Update()
    {
        if (isCount)
        {
            if (timeCount >= endDayCount)
            {
                isDayDone = true;
                var workers = FindObjectsOfType<WorkerMovement>();

                IEnumerable<WorkerMovement> workersNotAtHome = workers.Where(worker => !worker.isHome);

                if (workersNotAtHome.Count() == 0)
                {
                    StartNightTimeCountdown();
                }
            }
            timeCount += timeIncreaseModifier * Time.deltaTime;
        }
    }

    public void StartNightTimeCountdown()
    {
        // GameEventBus.Publish(GameState.DayEndGraphics);

        continueDayButton.SetActive(false);

        isCount = false;
        dayNightOverlay.SetActive(true);

        LeanTween.value(dayNightOverlay, 0, 1, 1).setOnUpdate((float val) =>
        {
            Color c = dayNightOverlayColor.color;
            c.a = val;
            dayNightOverlayColor.color = c;
        }).setOnComplete(() =>
        {
            // TODO: Add TEXT
            timeCount = 0;
            dayCount++;
            Helpers.SetTimeToZero();
            continueDayButton.SetActive(true);
        });
    }

    public void StartDayTimeCountDown()
    {
        isDayDone = false;
        UIManager.Instance.UpdateDayCountText();

        continueDayButton.SetActive(false);
        Helpers.ControlTime();

        LeanTween.value(dayNightOverlay, 1f, 0f, 1f).setOnUpdate((float val) =>
        {
            Color c = dayNightOverlayColor.color;
            c.a = val;
            dayNightOverlayColor.color = c;
        }).setOnComplete(() =>
        {
            isCount = true;
            WorkerManager.Instance.AllWorkersGoToWork();
            dayNightOverlay.SetActive(false);
            // Call all the events for the start of the day
            GameEventBus.Publish(GameState.DayStart);
        });

    }
}
