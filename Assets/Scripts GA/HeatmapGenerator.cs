using System.Collections.Generic;
using UnityEngine;

public class HeatmapGenerator : MonoBehaviour
{
    public GameObject cubeGO;
    public HeatmapType type = HeatmapType.POSITIONS;
    public float cellSize = 2.5f;
    public float limitedMaxValue = 100f;
    public bool debug = false;

    Heatmap heatmap;

    public Gradient gradient = new()
    {
        alphaKeys = new[]
        {
            new GradientAlphaKey(0.5f, 0f),
            new GradientAlphaKey(1f, 1f)
        },

        colorKeys = new[]
        {
            new GradientColorKey(Color.green, 0f),
            new GradientColorKey(Color.yellow, 0.5f),
            new GradientColorKey(Color.red, 1f)
        }
    };

    public void GenerateHeatmap(List<SpatialData> positionList, List<SpatialData> dataTypeList = null)
    {
        heatmap = new Heatmap(type, (int)(150f / cellSize), (int)(100f / cellSize), cellSize, new Vector3(-50, 0, -50), limitedMaxValue, gradient, cubeGO, debug, positionList);
    }
}