using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GlobalEvent
{
    AllDead,
    AllRun,
    AllIdle,
    AllSpin,
    AllAttack
}

public class EventBus : MonoBehaviour
{
    static bool IsApplicationQuitting = false;
    static EventBus instance;
    public static EventBus Instance
    {
        get
        {
            if (instance == null && !IsApplicationQuitting)
            {
                Debug.Log("Event bus Instantiate");
                GameObject go = new GameObject("EventBus");
                instance =  go.AddComponent<EventBus>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GetSomething()
    {
        Debug.Log("Hello");
    }

    private Dictionary<GlobalEvent, Action> eventDictionary = new Dictionary<GlobalEvent, Action>();

    public void Subscribe(GlobalEvent eventType, Action listener)
    {
        // 1번
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = null;
        }
        eventDictionary[eventType] += listener;

        // 2번 (1번과 동일)
        // if (eventDictionary.ContainsKey(eventType))
        // {
        //     eventDictionary[eventType] += listener;
        // }
        // else
        // {
        //     eventDictionary[eventType] = listener;
        // }
    }

    public void Unsubscribe(GlobalEvent eventType, Action listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;

            if (eventDictionary[eventType] == null)
            {
                eventDictionary.Remove(eventType);
            }
        }
    }

    public void Publish(GlobalEvent eventType)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType]?.Invoke();
        }
    }

    void OnApplicationQuit()
    {
        IsApplicationQuitting = true;
    }
}
