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

public struct CellData
{
    public float value;
    public float height;
}

public class Heatmap
{
    HeatmapType type = HeatmapType.POSITIONS;

    float HEAT_MAP_MAX_VALUE = 100f;

    CellData[,] HeatmapGrid;
    CellData[,] eventGrid;

    int width;
    int height;
    float cellSize;
    Vector3 originPosition;
    Gradient gradient;

    bool debugging = false;
    TextMesh[,] debugTextArray;

    GameObject[,] cubeArray;

    public Heatmap(HeatmapType type, int width, int height, float cellSize, Vector3 originPosition, float limitedMaxValue, Gradient gradient, GameObject cubeGO, bool debugging, List<SpatialData> positionList, List<SpatialData> eventList = null)
    {
        this.type = type;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        HEAT_MAP_MAX_VALUE = limitedMaxValue;
        this.gradient = gradient;

        HeatmapGrid = new CellData[width, height];

        debugTextArray = new TextMesh[width, height];
        this.debugging = debugging;

        cubeArray = new GameObject[width, height];

        if (debugging)
        {
            for (int x = 0; x < HeatmapGrid.GetLength(0); x++)
            {
                for (int y = 0; y < HeatmapGrid.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(HeatmapGrid[x, y].value.ToString("F3"), null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 8, Color.black, TextAnchor.MiddleCenter);
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
            GetGridMaxValue(HeatmapGrid);
        }

        // Normalize the heatmap values
        NormalizeGridValues(HeatmapGrid);

        if ((type == HeatmapType.KILLS || type == HeatmapType.DEATHS) && eventList != null)
        {
            eventGrid = new CellData[width, height];

            // Set grid values
            SetEventGridValues(eventList, eventGrid);

            if (HEAT_MAP_MAX_VALUE == 100f)
            {
                // Get the heatmap max value in order to normalize the values afterwards
                GetGridMaxValue(eventGrid);
            }

            // Normalize the event grid values
            NormalizeGridValues(eventGrid);
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

    void SetValue(int x, int y, float value, CellData[,] grid)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            grid[x,y].value = value;

            if (debugging)
                debugTextArray[x, y].text = HeatmapGrid[x, y].value.ToString("F3");
        }
    }

    void SetValue(Vector3 worldPosition, float value, CellData[,] grid)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value, grid);
    }

    float GetValue(int x, int y, CellData[,] grid)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return grid[x, y].value;
        }
        else
        {
            return 0f;
        }
    }

    float GetValue(Vector3 worldPosition, CellData[,] grid)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y, grid);
    }

    void SetHeight(int x, int y, float height, CellData[,] grid)
    {
        grid[x, y].height = height;
    }

    // Set grid values
    void SetGridValues(List<SpatialData> spatialDataList)
    {
        foreach (SpatialData spatialData in spatialDataList)
        {
            int x, y;
            GetXY(spatialData.position, out x, out y);

            float value = GetValue(x, y, HeatmapGrid) + spatialData.deltaTime;
            SetValue(x, y, value, HeatmapGrid);

            float height;
            if (HeatmapGrid[x, y].height != 0)
                height = (HeatmapGrid[x, y].height + spatialData.position.y) / 2;
            else
                height = spatialData.position.y;

            SetHeight(x, y, height, HeatmapGrid);
        }
    }

    void SetEventGridValues(List<SpatialData> spatialDataList, CellData[,] grid)
    {
        foreach (SpatialData spatialData in spatialDataList)
        {
            int x, y;
            GetXY(spatialData.position, out x, out y);

            float value = GetValue(x, y, grid) + 1;
            SetValue(x, y, value, grid);

            float height;
            if (grid[x, y].height != 0)
                height = (grid[x, y].height + spatialData.position.y) / 2;
            else
                height = spatialData.position.y;

            SetHeight(x, y, height, grid);
        }
    }

    // Get the heatmap max value in order to normalize the values afterwards
    void GetGridMaxValue(CellData[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                float value = grid[x, y].value;
                if (value > HEAT_MAP_MAX_VALUE)
                    HEAT_MAP_MAX_VALUE = value;
            }
        }
    }

    void NormalizeGridValues(CellData[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                float value = grid[x, y].value;
                value /= HEAT_MAP_MAX_VALUE;
                SetValue(x, y, value, grid);
            }
        }
    }

    // Generate the cubes in their position and with their proper color
    void GenerateCubes(GameObject cubeGO)
    {
        for (int x = 0; x < HeatmapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < HeatmapGrid.GetLength(1); y++)
            {
                float value;

                if (type == HeatmapType.KILLS || type == HeatmapType.DEATHS)
                    value = HeatmapGrid[x, y].value * eventGrid[x, y].value;
                else
                    value = HeatmapGrid[x, y].value;

                if (value != 0.0f || debugging)
                {
                    GameObject cube = GameObject.Instantiate(cubeGO);
                    cube.transform.localScale *= cellSize;

                    if (type == HeatmapType.KILLS || type == HeatmapType.DEATHS)
                        cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f + new Vector3(0, eventGrid[x, y].height + cellSize * .5f, 0);
                    else
                        cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f + new Vector3(0, HeatmapGrid[x, y].height + cellSize * .5f, 0);

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

                    cubeArray[x, y] = cube;
                }
            }
        }
    }

    // Destroy the cubes
    public void ClearCubes()
    {
        for (int x = 0; x < HeatmapGrid.GetLength(0); x++)
        {
            for (int y = 0; y < HeatmapGrid.GetLength(1); y++)
            {
                GameObject cube = cubeArray[x, y];
                if (cube != null)
                {
                    GameObject.Destroy(cube);
                }
            }
        }
    }
}