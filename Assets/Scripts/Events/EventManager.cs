using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages connections between event listeners and event invokers
// so objects can interact with each other without knowing each other
public static class EventManager {
    // ======================================================================
    // Field Variables
    // ======================================================================

    // list of invokers because we might have multiple invokers for a particular event
    // the EventName gives us a way to map between invokers and listeners as we're adding
    // invokers and adding listeners through the EventManager
    private static readonly Dictionary<EventName, List<FloatEventInvoker>> Invokers =
        new Dictionary<EventName, List<FloatEventInvoker>>();

    private static readonly Dictionary<EventName, List<UnityAction<float>>> Listeners =
        new Dictionary<EventName, List<UnityAction<float>>>();

    // ======================================================================
    // Public Methods
    // ======================================================================

    // Initializes the event manager
    public static void Initialize() {
        // create empty lists for all the dictionary entries
        // foreach goes through each of those five values in EventName enumeration
        foreach (EventName name in Enum.GetValues(typeof(EventName))) {
            // if the dictionary doesn't have that name already
            // creates new lists for the invokers and listeners
            if (!Invokers.ContainsKey(name)) {
                Invokers.Add(name, new List<FloatEventInvoker>());
                Listeners.Add(name, new List<UnityAction<float>>());
            } else {
                // if it already has the name, just clear the list
                // we clear the list because the `Initialize` method might be called
                // multiple times as we play the game
                // we don't want to try to add a new list if the dictionary already does contain
                // a particular name, because it throws an exception when trying to add something
                // with the same key as the dictionary already has
                Invokers[name].Clear();
                Listeners[name].Clear();
            }
        }
    }

    // Adds the given invoker for the given event name
    public static void AddInvoker(EventName eventName, FloatEventInvoker invoker) {
        // add listeners to new invoker and add new invoker to dictionary
        foreach (UnityAction<float> listener in Listeners[eventName]) {
            invoker.AddListener(eventName, listener);
        }

        Invokers[eventName].Add(invoker);
    }

    // Adds the given listener for the given event name
    public static void AddListener(EventName eventName, UnityAction<float> listener) {
        // add as listener to all invokers and add new listener to dictionary
        foreach (FloatEventInvoker invoker in Invokers[eventName]) {
            invoker.AddListener(eventName, listener);
        }

        Listeners[eventName].Add(listener);
    }

    // Removes the given invoker for the given event name
    // this increase the code efficiency when the invoker has been destroyed
    public static void RemoveInvoker(EventName eventName, FloatEventInvoker invoker) {
        // remove invoker from dictionary
        Invokers[eventName].Remove(invoker);
    }
}