using UnityEngine;
using System;
using System.Collections.Generic;

namespace SesiDefense.Core
{
    /// <summary>
    /// Central event system for decoupled communication between game systems.
    /// Uses generics for type-safe event dispatching.
    /// </summary>
    public class EventSystem : MonoBehaviour
    {
        private Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();
        private Queue<(Type eventType, object eventData)> eventQueue = new Queue<(Type, object)>();
        private bool isProcessingEvents = false;

        /// <summary>
        /// Registers a listener for a specific event type.
        /// </summary>
        public void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : class
        {
            Type eventType = typeof(TEvent);

            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Delegate>();
            }

            eventListeners[eventType].Add(listener);
        }

        /// <summary>
        /// Unregisters a listener for a specific event type.
        /// </summary>
        public void Unsubscribe<TEvent>(Action<TEvent> listener) where TEvent : class
        {
            Type eventType = typeof(TEvent);

            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(listener);
            }
        }

        /// <summary>
        /// Dispatches an event with a default constructor.
        /// </summary>
        public void Dispatch<TEvent>() where TEvent : class, new()
        {
            Dispatch<TEvent>(new TEvent());
        }

        /// <summary>
        /// Dispatches an event with provided data.
        /// </summary>
        public void Dispatch<TEvent>(TEvent eventData) where TEvent : class
        {
            Type eventType = typeof(TEvent);

            if (!eventListeners.ContainsKey(eventType))
            {
                return;
            }

            List<Delegate> listeners = eventListeners[eventType];

            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i] is Action<TEvent> listener)
                {
                    try
                    {
                        listener?.Invoke(eventData);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error in event listener: {ex.Message}", LogLevel.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Queues an event to be dispatched later.
        /// </summary>
        public void QueueEvent<TEvent>(TEvent eventData) where TEvent : class
        {
            eventQueue.Enqueue((typeof(TEvent), eventData));
        }

        /// <summary>
        /// Processes all queued events.
        /// </summary>
        public void ProcessQueuedEvents()
        {
            if (isProcessingEvents) return;

            isProcessingEvents = true;

            while (eventQueue.Count > 0)
            {
                var (eventType, eventData) = eventQueue.Dequeue();

                if (eventListeners.ContainsKey(eventType))
                {
                    List<Delegate> listeners = eventListeners[eventType];

                    for (int i = listeners.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            var invokeMethod = eventType.GetMethod("Invoke");
                            listeners[i].DynamicInvoke(eventData);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Error processing queued event: {ex.Message}", LogLevel.Error);
                        }
                    }
                }
            }

            isProcessingEvents = false;
        }

        /// <summary>
        /// Clears all event listeners.
        /// </summary>
        public void ClearAllListeners()
        {
            eventListeners.Clear();
            eventQueue.Clear();
        }

        /// <summary>
        /// Clears listeners for a specific event type.
        /// </summary>
        public void ClearListeners<TEvent>() where TEvent : class
        {
            Type eventType = typeof(TEvent);
            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Clear();
            }
        }

        /// <summary>
        /// Gets the count of listeners for a specific event type.
        /// </summary>
        public int GetListenerCount<TEvent>() where TEvent : class
        {
            Type eventType = typeof(TEvent);
            return eventListeners.ContainsKey(eventType) ? eventListeners[eventType].Count : 0;
        }

        private void OnDestroy()
        {
            ClearAllListeners();
        }
    }
}
