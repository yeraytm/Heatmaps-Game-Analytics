using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public abstract class EventData
{
    string url;

    public string type;

    public abstract string GetSerialize();
}

public class SpatialData : EventData
{
    float floatRounding = 100f;

    public uint playerID;
    public float x, y, z;
    public DateTime dateTime;

    public SpatialData(string type, uint playerID, Vector3 position, DateTime dateTime)
    {
        this.type = type;
        this.playerID = playerID;

        x = Mathf.Round(position.x * floatRounding) / floatRounding;
        y = Mathf.Round(position.y * floatRounding) / floatRounding;
        z = Mathf.Round(position.z * floatRounding) / floatRounding;

        this.dateTime = dateTime;
    }

    public override string GetSerialize()
    {
        return JsonUtility.ToJson(this);

        //form = new WWWForm();

        //form.AddField("Type", type);

        //form.AddField("PlayerID", playerID.ToString());

        //form.AddField("Position.x", position.x.ToString("0.00"));
        //form.AddField("Position.y", position.y.ToString("0.00"));
        //form.AddField("Position.z", position.z.ToString("0.00"));

        //form.AddField("Date", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //return form;
    }
}

public class AnalyticsController : MonoBehaviour
{
    string db = "https://citmalumnes.upc.es/~yeraytm/spatial_data.php";

    void OnEnable()
    {
        DataCollector.PositionEvent += SendEvent;
        DataCollector.KillEvent += SendEvent;
        DataCollector.DeathEvent += SendEvent;
    }

    void SendEvent(EventData data)
    {
        StartCoroutine(Send(data));
    }

    IEnumerator Send(EventData data)
    {
        UnityWebRequest request = UnityWebRequest.Post(db, data.GetSerialize());
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");

        //WWW request = new WWW(db, data.GetSerialize());

        yield return request.SendWebRequest();

        // Check for errors
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log("SENT REQUEST SUCCESS: " + request.responseCode);
        }
        else
        {
            Debug.Log("SENT REQUEST ERROR: " + request.error);
        }
    }
}
