using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public abstract class EventData
{
    string url;

    public string type;

    public abstract WWWForm Serialize();
}

public class SpatialData : EventData
{
    public uint playerID;
    public Vector3 position;
    public DateTime date;

    public SpatialData(string type, uint playerID, Vector3 pos, DateTime date)
    {
        this.type = type;
        this.playerID = playerID;

        position.x = pos.x;
        position.y = pos.y;
        position.z = pos.z;

        this.date = date;
    }

    public override WWWForm Serialize()
    {
        WWWForm form = new WWWForm();

        form.AddField("Type", type);

        form.AddField("PlayerID", playerID.ToString());

        form.AddField("PositionX", position.x.ToString("F2"));
        form.AddField("PositionY", position.y.ToString("F2"));
        form.AddField("PositionZ", position.z.ToString("F2"));

        form.AddField("Date", date.ToString("yyyy-MM-dd HH:mm:ss"));

        return form;

        //string ret = JsonUtility.ToJson(this);
        //return ret;
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
        Debug.Log("SENDING " + data.type + " EVENT");
        StartCoroutine(Send(data));
    }

    IEnumerator Send(EventData data)
    {
        WWW request = new WWW(db, data.Serialize());

        yield return request;

        // Check for errors
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log("SENT REQUEST SUCCESS: " + request.text);
        }
        else
        {
            Debug.Log("SENT REQUEST ERROR: " + request.error);
        }

        request.Dispose();

        //UnityWebRequest request = UnityWebRequest.Post(db, data.Serialize());
        //request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.SendWebRequest();

        //// Check for errors
        //if (string.IsNullOrEmpty(request.error))
        //{
        //    Debug.Log("SENT REQUEST SUCCESS: " + request.responseCode);
        //}
        //else
        //{
        //    Debug.Log("SENT REQUEST ERROR: " + request.error);
        //}
    }
}
