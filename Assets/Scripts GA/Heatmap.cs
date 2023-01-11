using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public enum HeatmapType
{
    POSITIONS,
    KILLS,
    DEATHS
}

public class Heatmap
{
    HeatmapType type = HeatmapType.POSITIONS;

    float HEAT_MAP_MAX_VALUE = 100f;

    float[,] positionGrid;
    float[,] dataTypeGrid;

    int width;
    int height;
    float cellSize;
    Vector3 originPosition;
    Gradient gradient;
    float[,] heightArray;

    bool debugging = false;
    TextMesh[,] debugTextArray;

    public Heatmap(HeatmapType type, int width, int height, float cellSize, Vector3 originPosition, float limitedMaxValue, Gradient gradient, GameObject cubeGO, bool debugging, List<SpatialData> positionList, List<SpatialData> dataTypeList = null)
    {
        this.type = type;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        HEAT_MAP_MAX_VALUE = limitedMaxValue;
        this.gradient = gradient;

        positionGrid = new float[width, height];
        heightArray = new float[width, height];

        debugTextArray = new TextMesh[width, height];
        this.debugging = debugging;

        if (debugging)
        {
            for (int x = 0; x < positionGrid.GetLength(0); x++)
            {
                for (int y = 0; y < positionGrid.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(positionGrid[x, y].ToString("F3"), null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 8, Color.black, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);
        }

        // Set grid values
        SetGridValues(positionList);

        if (HEAT_MAP_MAX_VALUE == 100f)
        {
            // Get the heatmap max value in order to normalize the values afterwards
            GetHeatmapMaxPositionValue();
        }

        // Normalize the heatmap values
        NormalizeHeatmapPositionValues();

        if (type == HeatmapType.KILLS)
        {

        }
        else if (type == HeatmapType.DEATHS)
        {

        }

        // Generate the cubes in their position and with their proper color
        GenerateCubes(cubeGO);
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    void SetValue(int x, int y, float value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            positionGrid[x,y] = value;

            if (debugging)
                debugTextArray[x, y].text = positionGrid[x, y].ToString("F3");
        }
    }

    void SetValue(Vector3 worldPosition, float value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    float GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return positionGrid[x, y];
        }
        else
        {
            return 0f;
        }
    }

    float GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    void SetHeight(int x, int y, float height)
    {
        heightArray[x, y] = height;
    }

    // Set grid values
    void SetGridValues(List<SpatialData> spatialDataList)
    {
        foreach (SpatialData spatialData in spatialDataList)
        {
            int x, y;
            GetXY(spatialData.position, out x, out y);

            float value = GetValue(x, y) + spatialData.deltaTime;
            SetValue(x, y, value);

            float height;
            if (heightArray[x, y] != 0)
                height = (heightArray[x, y] + spatialData.position.y) / 2;
            else
                height = spatialData.position.y;

            SetHeight(x, y, height);
        }
    }

    // Get the heatmap max value in order to normalize the values afterwards
    void GetHeatmapMaxPositionValue()
    {
        for (int x = 0; x < positionGrid.GetLength(0); x++)
        {
            for (int y = 0; y < positionGrid.GetLength(1); y++)
            {
                float value = positionGrid[x, y];
                if (value > HEAT_MAP_MAX_VALUE)
                    HEAT_MAP_MAX_VALUE = value;
            }
        }
    }

    void NormalizeHeatmapPositionValues()
    {
        for (int x = 0; x < positionGrid.GetLength(0); x++)
        {
            for (int y = 0; y < positionGrid.GetLength(1); y++)
            {
                float value = positionGrid[x, y];
                value /= HEAT_MAP_MAX_VALUE;
                SetValue(x, y, value);
            }
        }
    }

    // Generate the cubes in their position and with their proper color
    void GenerateCubes(GameObject cubeGO)
    {
        for (int x = 0; x < positionGrid.GetLength(0); x++)
        {
            for (int y = 0; y < positionGrid.GetLength(1); y++)
            {
                float value = positionGrid[x, y];

                if (value != 0.0f || debugging)
                {
                    GameObject cube = GameObject.Instantiate(cubeGO);
                    cube.transform.localScale *= cellSize;

                    cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f + new Vector3(0, heightArray[x, y] + cellSize * .5f, 0);

                    if (value != 0.0f)
                    {
                        // Setting up the gradient color
                        Color gradientColor = gradient.Evaluate(value);
                        cube.GetComponent<Renderer>().material.color = gradientColor;
                    }
                    else
                    {
                        cube.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f, 0.25f);
                    }
                }
            }
        }
    }
}