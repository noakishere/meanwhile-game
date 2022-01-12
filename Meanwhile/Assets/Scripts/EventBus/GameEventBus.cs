using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameEventBus
{
    private static readonly IDictionary<GameState, UnityEvent> Events = new Dictionary<GameState, UnityEvent>();

    public static void Subscribe(GameState gameState, UnityAction listener)
    {
        UnityEvent thisEvent;
        if (Events.TryGetValue(gameState, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Events.Add(gameState, thisEvent);
        }
    }

    public static void Unsubscribe(GameState gameState, UnityAction listener)
    {
        UnityEvent thisEvent;

        if (Events.TryGetValue(gameState, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(GameState gameState)
    {
        UnityEvent thisEvent;

        if (Events.TryGetValue(gameState, out thisEvent))
        {
            thisEvent?.Invoke();
        }
    }

}
