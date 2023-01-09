using System;
using UnityEngine;
using Gamekit3D;

public class DataCollector : MonoBehaviour
{
    public uint playerID = 0;

    public Transform player;

    public static event Action<EventData> PositionEvent;
    public static event Action<EventData> KillEvent;
    public static event Action<EventData> DeathEvent;

    float elapsedUpdateTime = 0f;

    void Start()
    {
    }

    void Update()
    {
        //elapsedUpdateTime += Time.deltaTime;
        //if (elapsedUpdateTime >= 4.0f)
        //{
        //    Debug.Log(player.transform.position);
        //    PositionEvent?.Invoke(new SpatialData("Position", playerID, player.transform.position, DateTime.Now));
        //    elapsedUpdateTime = elapsedUpdateTime % 1f;
        //}
    }

    public void TriggerKillEvent()
    {
        Debug.Log("KILL EVENT IS TRIGGERED!");
        Debug.Log(player.position);
        SpatialData killData = new SpatialData("Kill", playerID, player.position, DateTime.Now);
        KillEvent?.Invoke(killData);
    }

    public void TriggerDeathEvent()
    {
        Debug.Log("DEATH EVENT IS TRIGGERED!");
        Debug.Log(player.position);
        DeathEvent?.Invoke(new SpatialData("Death", playerID, player.position, DateTime.Now));
    }
}