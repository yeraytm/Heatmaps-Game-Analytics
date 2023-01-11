using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeatmapGenerator : MonoBehaviour
{
    public GameObject cubeGO;
    public HeatmapType type = HeatmapType.POSITIONS;
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
    [SerializeField] Button positionsButton;
    [SerializeField] Button killsButton;
    [SerializeField] Button deathsButton;

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

    // Start is called before the first frame update
    void Start()
    {
        cellSizeSlider.onValueChanged.AddListener((v) =>
        {
            cellSize = v;
            if (heatmap != null)
                heatmap.ClearCubes();
            GenerateHeatmap();

            cellSizeSliderValue.text = v.ToString("0");
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
}