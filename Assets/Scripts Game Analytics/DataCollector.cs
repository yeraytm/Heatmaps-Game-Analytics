using System;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    Transform playerTransform;

    uint playerID;

    public static event Action<EventData> PositionEvent;
    public static event Action<EventData> KillEvent;
    public static event Action<EventData> DeathEvent;

    float elapsedUpdateTime = 0f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        elapsedUpdateTime += Time.deltaTime;
        if (elapsedUpdateTime >= 1.0f)
        {
            PositionEvent?.Invoke(new SpatialData("Position", playerID, playerTransform.position, DateTime.Now));
            elapsedUpdateTime = elapsedUpdateTime % 1f;
        }
    }

    public void TriggerKillEvent()
    {
        Debug.Log("KILL EVENT IS TRIGGERED!");
        KillEvent?.Invoke(new SpatialData("Kill", playerID, playerTransform.position, DateTime.Now));
    }

    public void TriggerDeathEvent()
    {
        Debug.Log("DEATH EVENT IS TRIGGERED!");
        KillEvent?.Invoke(new SpatialData("Death", playerID, playerTransform.position, DateTime.Now));
    }
}