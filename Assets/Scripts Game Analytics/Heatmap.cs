using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Heatmap
{
    public float HEAT_MAP_MAX_VALUE = 100f;
    public const float HEAT_MAP_MIN_VALUE = 0f;

    float[,] gridArray;

    int width;
    int height;

    float cellSize;

    Vector3 originPosition;

    Gradient gradient;

    bool debugging = false;
    TextMesh[,] debugTextArray;

    public Heatmap(int width, int height, float cellSize, Vector3 originPosition, Gradient gradient, List<SpatialData> spatialDataList, GameObject cubeGO, bool debugging)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gradient = gradient;

        gridArray = new float[width, height];

        debugTextArray = new TextMesh[width, height];
        this.debugging = debugging;

        // Set grid values
        SetGridValues(spatialDataList);

        if (HEAT_MAP_MAX_VALUE == 100f)
        {
            // Get the heatmap max value in order to normalize the values afterwards
            GetHeatmapMaxValue();
        }

        // Generate the cubes in their position and with their proper color
        GenerateCubes(cubeGO);

        if (debugging)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 8, Color.black, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetValue(int x, int y, float value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (value > HEAT_MAP_MAX_VALUE)
                HEAT_MAP_MAX_VALUE = value;
            
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);

            if (debugging)
                debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, float value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public float GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0f;
        }
    }

    public float GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    // Set grid values
    private void SetGridValues(List<SpatialData> spatialDataList)
    {
        foreach (SpatialData spatialData in spatialDataList)
        {
            int x, y;
            GetXY(spatialData.position, out x, out y);

            float value = GetValue(x, y) + spatialData.deltaTime;
            SetValue(x, y, value);
        }
    }

    // Get the heatmap max value in order to normalize the values afterwards
    private void GetHeatmapMaxValue()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                float value = gridArray[x, y];
                if (value > HEAT_MAP_MAX_VALUE)
                    HEAT_MAP_MAX_VALUE = value;
            }
        }
    }

    // Generate the cubes in their position and with their proper color
    private void GenerateCubes(GameObject cubeGO)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                float value = gridArray[x, y];
                value /= HEAT_MAP_MAX_VALUE;
                SetValue(x, y, value);

                if (value != 0.0f || debugging)
                {
                    GameObject cube = GameObject.Instantiate(cubeGO);
                    cube.transform.localScale *= cellSize;
                    cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;

                    // Setting up the gradient color
                    Color gradientColor = gradient.Evaluate(value);
                    cube.GetComponent<Renderer>().material.color = gradientColor;
                }
            }
        }
    }
}