using System;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    public uint playerID = 0;

    public Transform player;

    public static event Action<EventData> SpatialEvent;

    float elapsedUpdateTime = 0f;

    void Update()
    {
        elapsedUpdateTime += Time.deltaTime;
        if (elapsedUpdateTime >= 0.5f)
        {
            SpatialData positionData = new SpatialData("Position", playerID, player.transform.position, DateTime.Now);
            SpatialEvent?.Invoke(positionData);

            elapsedUpdateTime = elapsedUpdateTime % 1f;
        }
    }

    public void TriggerKillEvent()
    {
        SpatialData killData = new SpatialData("Kill", playerID, player.position, DateTime.Now);
        SpatialEvent?.Invoke(killData);
    }

    public void TriggerDeathEvent()
    {
        SpatialData deathData = new SpatialData("Death", playerID, player.position, DateTime.Now);
        SpatialEvent?.Invoke(deathData);
    }
}