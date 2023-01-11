using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    // bool limitedMaxValue;
    // other customizable variables...

    public GameObject cubeGO;
    public float cellSize = 2.5f;
    public bool debug = false;
    Heatmap grid;

    public Gradient gradient = new()
    {
        alphaKeys = new[]
        {
            new GradientAlphaKey(0.5f, 0f),
            new GradientAlphaKey(0.5f, 1f)
        },

        colorKeys = new[]
        {
            new GradientColorKey(Color.green, 0f),
            new GradientColorKey(Color.blue, 0.5f),
            new GradientColorKey(Color.red, 1f)
        }
    };

    //void Start()
    //{
    //    grid = new Heatmap(44, 32, 2.5f, new Vector3(-35,0,-40), gradient, spatialDatas, cubeGO);
    //}

    public void GenerateHeatmap(List<SpatialData> spatialDataList)
    {
        grid = new Heatmap((int)(150f / cellSize), (int)(100f / cellSize), cellSize, new Vector3(-50, 0, -50), gradient, spatialDataList, cubeGO, debug);
    }
}