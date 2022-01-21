using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private Bloom bloom;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.DayStart, TurnBloomUp);
        GameEventBus.Subscribe(GameState.DayEndGraphics, TurnBloomDown);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.DayStart, TurnBloomUp);
        GameEventBus.Unsubscribe(GameState.DayEndGraphics, TurnBloomDown);
    }

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out bloom);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            TurnBloomDown();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            TurnBloomUp();
        }
    }

    public void TurnBloomDown()
    {
        LeanTween.value(this.gameObject, 5.85f, 0, 3).setOnUpdate((float val) =>
        {
            bloom.intensity.value = val;
        });
    }
    public void TurnBloomUp()
    {
        bloom.intensity.value = 0f;
        LeanTween.value(this.gameObject, 0, 5.85f, 3).setOnUpdate((float val) =>
        {
            bloom.intensity.value = val;
        });
    }
}
