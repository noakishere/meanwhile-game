using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    // This is used for different states where we need to control timeScale
    public static void ControlTime()
    {
        var condition = Time.timeScale == 1f ? Time.timeScale = 0f : Time.timeScale = 1f;
    }

    public static void SetTimeToZero()
    {
        Time.timeScale = 0f;
    }
}
