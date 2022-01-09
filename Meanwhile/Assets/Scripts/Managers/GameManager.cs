using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("Resources")]
    public int woods;

    // TEST, CLEAN UP LATER
    public TMP_Text woodsText;

    private void Update()
    {
        woodsText.text = $"Woods: {woods}";
    }

}
