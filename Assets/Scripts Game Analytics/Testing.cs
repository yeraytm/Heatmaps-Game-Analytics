using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    // bool limitedMaxValue;
    // other customizable variables...

    public GameObject playerGO;
    public GameObject cubeGO;

    public Gradient gradient = new Gradient
    {
        alphaKeys = new[]
        {
            new GradientAlphaKey(0.5f, 0f),
            new GradientAlphaKey(0.5f, 1f)
        },

        colorKeys = new[]
        {
            new GradientColorKey(Color.blue, 0f),
            new GradientColorKey(Color.green, 0.5f),
            new GradientColorKey(Color.red, 1f)
        }
    };

    public bool debug = false;

    Heatmap grid;

    //void Start()
    //{
    //    grid = new Heatmap(44, 32, 2.5f, new Vector3(-35,0,-40), gradient, spatialDatas, cubeGO);
    //}

    public void GenerateHeatmap(List<SpatialData> spatialDataList)
    {
        grid = new Heatmap(44, 32, 2.5f, new Vector3(-35, 0, -40), gradient, spatialDataList, cubeGO, debug);
    }
}