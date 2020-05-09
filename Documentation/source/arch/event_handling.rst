.. figure:: ../_static/index/cover.png
    :align: center

Event Handling Pattern
======================

The game design follows a simple observer pattern where event handlers responds when an event occurs. Unity Event Handling system has been based on the delegate type which specifies a method signature and allow us to pass references to methods. The design pattern are shown in the system diagram below.

.. figure:: ../_static/system_diagrams/event_handling_system_diagram.png
    :align: center

    System Diagram of Event Handling Design Pattern (``ctrl`` + ``+`` to zoom in)



Event Manager
-------------

The purpose of a centralised event manager script is to manage connections between event listeners and event invokers, therefore objects can interact without creating instances for the them to know about each other.

Rather than defining each invoker and corresponding listener, an ``enum`` of event names has been declared in a separate file to extract all the events and actions of the same data type:

.. code-block:: C#

    public enum EventName {
        HealthChangedEvent,
        SpeedUpActivatedEvent,
        GameOverEvent,
        TimerChangedEvent,
    }

Then corresponding classes of events have been declared in separate files such as ``HealthChangedEvent``:

.. code-block:: C#

    public class HealthChangedEvent : UnityEvent<float> { }

.. note:: For the ease of implementation, I declare all the event as one ``float`` argument event.

Then, in the ``EventManager.cs``, lists of invokers and listeners have been declared because we might have multiple invokers for a particular event:

.. code-block:: C#

    private static readonly Dictionary<EventName, List<FloatEventInvoker>> Invokers =
        new Dictionary<EventName, List<FloatEventInvoker>>();

    private static readonly Dictionary<EventName, List<UnityAction<float>>> Listeners =
        new Dictionary<EventName, List<UnityAction<float>>>();

Then we declare the ``Initalize()`` method to be called elsewhere when initalising the game session. 

We create empty lists for all the dictionary entries, ``foreach`` goes through each of those four values in ``EventName`` enumeration. If the dictionary doesn't have that name already, we create new lists for the invokers and listeners. If it alreayd has the name, we clear the list because ``Initalize()`` method might be called multiple times as we play the game. We don't want to try to add a new list if the dictionary already does contain a particular name, because it throws an exception when trying to add something with the same key as the dictionary already has.

.. code-block:: C#

    public static void Initialize() {
        foreach (EventName name in Enum.GetValues(typeof(EventName))) {
            if (!Invokers.ContainsKey(name)) {
                Invokers.Add(name, new List<FloatEventInvoker>());
                Listeners.Add(name, new List<UnityAction<float>>());
            } else {
                Invokers[name].Clear();
                Listeners[name].Clear();
            }
        }
    }

After that, we declare the float argument handlers to be called in listeners and invokers:

.. code-block:: C#

    // Adds the given invoker for the given event name with float argument
    public static void AddFloatArgInvoker(EventName eventName, FloatEventInvoker invoker) {
        // add listeners to new invoker and add new invoker to dictionary
        foreach (UnityAction<float> listener in Listeners[eventName]) {
            invoker.AddFloatArgListener(eventName, listener);
        }

        Invokers[eventName].Add(invoker);
    }

    // Adds the given listener for the given event name with float argument
    public static void AddFloatArgListener(EventName eventName, UnityAction<float> listener) {
        // add a listener to all invokers and add new listener to dictionary
        foreach (FloatEventInvoker invoker in Invokers[eventName]) {
            invoker.AddFloatArgListener(eventName, listener);
        }

        Listeners[eventName].Add(listener);
    }

Don't forget to add removal functionality of the invoker when the invoker has been destroyed or no longer interacts with and scene objects to increase the code efficiency.

.. code-block:: C#

    public static void RemoveFloatArgInvoker(EventName eventName, FloatEventInvoker invoker) {
        // remove invoker from dictionary
        Invokers[eventName].Remove(invoker);
    }



Invoker
-------