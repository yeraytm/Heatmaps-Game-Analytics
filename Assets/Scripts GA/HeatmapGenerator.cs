using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeatmapGenerator : MonoBehaviour
{
    public GameObject cubeGO;
    public HeatmapType type = HeatmapType.NONE;
    public float cellSize = 2.5f;
    public float limitedMaxValue = 100f;
    public bool debug = false;

    Heatmap heatmap;
    List<SpatialData> positionsList;
    List<SpatialData> killsList;
    List<SpatialData> deathsList;

    // UI
    [SerializeField] Slider cellSizeSlider;
    [SerializeField] TextMeshProUGUI cellSizeSliderValue;

    [SerializeField] Slider maxValueSlider;
    [SerializeField] TextMeshProUGUI maxValueSliderValue;

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


    void Start()
    {
        cellSizeSlider.onValueChanged.AddListener((v) =>
        {
            cellSize = v;

            if (heatmap != null)
                heatmap.ClearCubes();

            GenerateHeatmap();

            cellSizeSliderValue.text = v.ToString();
        });

        maxValueSlider.onValueChanged.AddListener((v) =>
        {
            limitedMaxValue = v;

            if (heatmap != null)
                heatmap.ClearCubes();

            GenerateHeatmap();

            maxValueSliderValue.text = v.ToString();
        });
    }

    public void Init(List<SpatialData> positionsList, List<SpatialData> killsList, List<SpatialData> deathsList)
    {
        this.positionsList = positionsList;
        this.killsList = killsList;
        this.deathsList = deathsList;
    }

    public void GenerateHeatmap()
    {
        switch (type)
        {
            case HeatmapType.POSITIONS:
                heatmap = new Heatmap(type, (int)(150f / cellSize), (int)(100f / cellSize), cellSize, new Vector3(-50, 0, -50), limitedMaxValue, gradient, cubeGO, debug, positionsList);
                break;
            case HeatmapType.KILLS:
                heatmap = new Heatmap(type, (int)(150f / cellSize), (int)(100f / cellSize), cellSize, new Vector3(-50, 0, -50), limitedMaxValue, gradient, cubeGO, debug, positionsList, killsList);
                break;
            case HeatmapType.DEATHS:
                heatmap = new Heatmap(type, (int)(150f / cellSize), (int)(100f / cellSize), cellSize, new Vector3(-50, 0, -50), limitedMaxValue, gradient, cubeGO, debug, positionsList, deathsList);
                break;
        }
    }

    public void ChangeHeatmapType(int type)
    {
        Debug.Log(type);
        if (this.type == (HeatmapType)type)
            return;

        this.type = (HeatmapType)type;

        if (heatmap != null)
            heatmap.ClearCubes();

        GenerateHeatmap();
    }
}