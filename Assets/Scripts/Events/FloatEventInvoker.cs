using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Extends MonoBehaviour to support one or more invoking one integer argument UnityEvents
public class FloatEventInvoker : MonoBehaviour {
    // look up values by using keys, the keys don't have to be strings but any data type
    // in this case, keys are enumerations and values int unity events
    // dictionary enables us to invoke more than one event
    protected Dictionary<EventName, UnityEvent<float>> UnityEvents =
        new Dictionary<EventName, UnityEvent<float>>();

    // Adds the given listener for the given event name
    public void AddFloatArgListener(EventName eventName, UnityAction<float> listener) {
        // only add listeners for supported events, `ContainsKey` check for the key
        if (UnityEvents.ContainsKey(eventName)) {
            // get the invoker by putting the key in between square brackets
            UnityEvents[eventName].AddListener(listener);
        }
    }
}