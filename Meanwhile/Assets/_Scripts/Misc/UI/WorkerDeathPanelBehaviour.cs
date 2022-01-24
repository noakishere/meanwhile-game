using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class WorkerDeathPanelBehaviour : MonoBehaviour, IPointerDownHandler
{
    private Vector3 initialPosition;

    private Image image;
    private GameObject textChild;
    [SerializeField] private TMP_Text panelText;

    private void Start()
    {
        initialPosition = gameObject.transform.position;
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.DayEndGraphics, Disappear);

        Appear();
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.DayEndGraphics, Disappear);
    }

    // Equivalent of OnMouseDown for UI elements 
    public void OnPointerDown(PointerEventData eventData)
    {
        Disappear();
    }

    public void Disappear()
    {
        foreach (Transform child in transform) Object.Destroy(child.gameObject);

        LeanTween.value(gameObject, 1f, 0f, 0.4f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        }).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void Appear()
    {
        LeanTween.value(gameObject, 0f, 1f, 0.5f).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        }).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 0f, 1f, 0.5f).setOnUpdate((float val) =>
            {
                Color c = panelText.color;
                c.a = val;
                panelText.color = c;
            });
        });
    }
}
