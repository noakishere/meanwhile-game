using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerHealthBarUIScript : MonoBehaviour
{
    public Slider slider { get; private set; }

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
