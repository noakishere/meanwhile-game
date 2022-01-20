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
            print("Day is done");
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
        LeanTween.value(dayNightOverlay, 1, 0, 3).setOnUpdate((float val) =>
        {
            Color c = dayNightOverlayColor.color;
            c.a = val;
            dayNightOverlayColor.color = c;
        });
        dayNightOverlay.SetActive(false);
        Helpers.ControlTime();
        isCount = true;
        WorkerManager.Instance.AllWorkersGoToWork();
    }
}
