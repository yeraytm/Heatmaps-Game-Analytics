using Gamekit3D;
using System;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    public static event Action<EventData> SpatialEvent;

    public uint playerID = 0;

    public PlayerController player;

    public float samplingRate = 1.5f;
    public float riskRadius = 6f;
    [SerializeField] LayerMask enemyColliderLayerMask;

    float elapsedUpdateTime = 0f;
    int nearbyEnemies = 0;

    bool once = true;

    void Update()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(player.transform.position, riskRadius, enemyColliderLayerMask);
        nearbyEnemies = enemyColliders.Length;

        if (nearbyEnemies <= 0)
            samplingRate = 1.25f;
        else
            samplingRate = 0.25f;

        elapsedUpdateTime += Time.deltaTime;
        if (elapsedUpdateTime >= samplingRate)
        {
            SpatialData positionData = new SpatialData("Position", playerID, player.transform.position, elapsedUpdateTime + samplingRate);
            SpatialEvent?.Invoke(positionData);

            elapsedUpdateTime = elapsedUpdateTime % 1f;
        }

        if (player.respawning)
        {
            if (once)
            {
                TriggerDeathEvent();
                once = false;
            }
        }
        else
            once = true;
    }

    public void TriggerKillEvent()
    {
        SpatialData killData = new SpatialData("Kill", playerID, player.transform.position, 0.000f);
        SpatialEvent?.Invoke(killData);
    }

    public void TriggerDeathEvent()
    {
        SpatialData deathData = new SpatialData("Death", playerID, player.transform.position, 0.000f);
        SpatialEvent?.Invoke(deathData);
    }
}