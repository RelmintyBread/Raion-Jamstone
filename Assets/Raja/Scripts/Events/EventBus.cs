using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> eventTable
        = new Dictionary<Type, Delegate>();

    // Subscribe: mendaftarkan listener untuk suatu event
    public static void Subscribe<T>(Action<T> listener)
    {
        if (listener == null)
            return;

        Type eventType = typeof(T);

        if (eventTable.TryGetValue(eventType, out Delegate existing))
        {
            eventTable[eventType] = Delegate.Combine(existing, listener);
        }
        else
        {
            eventTable[eventType] = listener;
        }
    }

    // Unsubscribe: menghapus listener dari suatu event
    public static void Unsubscribe<T>(Action<T> listener)
    {
        if (listener == null)
            return;

        Type eventType = typeof(T);

        if (!eventTable.TryGetValue(eventType, out Delegate existing))
            return;

        Delegate updated = Delegate.Remove(existing, listener);

        if (updated == null)
        {
            eventTable.Remove(eventType);
        }
        else
        {
            eventTable[eventType] = updated;
        }
    }

    // Publish: mengirim event ke seluruh listener
    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventTable.TryGetValue(eventType, out Delegate existing))
        {
            if (existing is Action<T> callback)
            {
                callback.Invoke(eventData);
            }
        }
    }

    // Menghapus seluruh listener
    public static void Clear()
    {
        eventTable.Clear();
    }
}