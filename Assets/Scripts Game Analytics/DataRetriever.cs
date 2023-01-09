using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataRetriever : MonoBehaviour
{
    public string db = "https://citmalumnes.upc.es/~yeraytm/query_spatial_data.php";
    uint count = 0;

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

                                Debug.Log(
                                    "Type: " + rowData[0] +
                                    " PlayerID: " + rowData[1] +
                                    " PositionX: " + rowData[2] +
                                    " PositionY: " + rowData[3] +
                                    " PositionZ: " + rowData[4] +
                                    " Date: " + rowData[5]
                                    );
                                count++;
                            }
                        }
                        break;
                    }
            }
        }
        Debug.Log("TOTAL ROWS: " + count);
    }
}