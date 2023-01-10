using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public abstract class EventData
{
    public string type;

    public abstract WWWForm Serialize();
}

public class SpatialData : EventData
{
    public uint playerID;
    public Vector3 position;
    public float deltaTime;

    public SpatialData(string type, uint playerID, Vector3 pos, float deltaTime)
    {
        this.type = type;
        this.playerID = playerID;

        position.x = pos.x;
        position.y = pos.y;
        position.z = pos.z;

        this.deltaTime = deltaTime;
    }

    public override WWWForm Serialize()
    {
        WWWForm form = new WWWForm();

        form.AddField("Type", type);

        form.AddField("PlayerID", playerID.ToString());

        form.AddField("PositionX", position.x.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
        form.AddField("PositionY", position.y.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
        form.AddField("PositionZ", position.z.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));

        form.AddField("DeltaTime", deltaTime.ToString("F3", System.Globalization.CultureInfo.InvariantCulture));

        return form;
    }
}

public class AnalyticsController : MonoBehaviour
{
    public string db = "https://citmalumnes.upc.es/~yeraytm/spatial_data.php";

    void OnEnable()
    {
        DataCollector.SpatialEvent += SendEvent;
    }

    void SendEvent(EventData data)
    {
        StartCoroutine(Send(data));
    }

    IEnumerator Send(EventData data)
    {
        UnityWebRequest request = UnityWebRequest.Post(db, data.Serialize());

        yield return request.SendWebRequest();

        // Check for errors
        if (string.IsNullOrEmpty(request.error))
        {
            Debug.Log("[REQUEST SUCCESS] Response Code: " + request.responseCode + '\n' + "[REQUEST SUCCESS] Ouput: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("[REQUEST ERROR] Error Code: " + request.error);
        }

        request.Dispose();
    }
}