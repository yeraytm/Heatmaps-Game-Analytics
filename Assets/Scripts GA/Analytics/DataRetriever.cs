using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class DataRetriever : MonoBehaviour
{
    public string db = "https://citmalumnes.upc.es/~yeraytm/query_spatial_data.php";
    uint count = 0;

    List<SpatialData> positionsList = new List<SpatialData>();
    List<SpatialData> killsList = new List<SpatialData>();
    List<SpatialData> deathsList = new List<SpatialData>();

    public static event Action<List<SpatialData>, List<SpatialData>> SpatialEvent;

    void Start()
    {
        StartCoroutine(GetRequest(db));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("[CONNECTION ERROR] Error Code:" + request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("[REQUEST ERROR] Error Code: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("[HTTP REQUEST ERROR] Error Code: " + request.error);
                    break;
                case UnityWebRequest.Result.Success:
                    {
                        string requestRespone = request.downloadHandler.text;

                        string[] rows = requestRespone.Split('*');

                        for (int i = 0; i < rows.Length; i++)
                        {
                            if (rows[i] != "")
                            {
                                string[] rowData = rows[i].Split('#');

                                float x = float.Parse(rowData[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                float y = float.Parse(rowData[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                float z = float.Parse(rowData[4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                Vector3 position = new Vector3(x, y, z);

                                SpatialData entry = new SpatialData(
                                    rowData[0],
                                    uint.Parse(rowData[1]),
                                    position,
                                    float.Parse(rowData[5], System.Globalization.CultureInfo.InvariantCulture.NumberFormat)
                                    );

                                switch (entry.type)
                                {
                                    case "Position":
                                        positionsList.Add(entry);
                                        break;
                                    case "Kill":
                                        killsList.Add(entry);
                                        break;
                                    case "Death":
                                        deathsList.Add(entry);
                                        break;
                                }
                                count++;
                            }
                        }
                        break;
                    }
            }
        }
        Debug.Log("TOTAL ROWS RETRIEVED: " + count);

        gameObject.GetComponent<HeatmapGenerator>().Init(positionsList, killsList, deathsList);
    }
}