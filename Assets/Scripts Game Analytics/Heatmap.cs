using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Heatmap
{
    public int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private Vector3[] positionArray = new Vector3[0]; // The actual data!
    private Gradient gradient;

    public Heatmap(int width, int height, float cellSize, Vector3 originPosition, Gradient gradient, GameObject cubeGO)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gradient = gradient;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 20, Color.black, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);

                //GameObject cube = GameObject.Instantiate(cubeGO);
                //cube.transform.localScale *= cellSize;
                //cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;

                //cube.GetComponent<Renderer>().material.color = new Color(1,1,1,0.5f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

        // Get the heatmap max value in order to normalize the values afterwards
        GetHeatmapMaxValue(positionArray);

        // Generate the cubes in their position and with their proper color
        GenerateCubes(cubeGO, positionArray);
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

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (value > HEAT_MAP_MAX_VALUE)
                HEAT_MAP_MAX_VALUE = value;
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            debugTextArray[x, y].text = gridArray[x,y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    // Increment cell value
    public void IncrementValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, GetValue(x, y) + 1);
    }

    // Get the heatmap max value in order to normalize the values afterwards
    private void GetHeatmapMaxValue(Vector3[] positionArray)
    {
        foreach (Vector3 v in positionArray)
        {
            int x, y;
            GetXY(v, out x, out y);

            int value = GetValue(x, y);
            if (value > HEAT_MAP_MAX_VALUE)
                HEAT_MAP_MAX_VALUE = value;
        }
    }

    // Generate the cubes in their position and with their proper color
    private void GenerateCubes(GameObject cubeGO, Vector3[] positionArray)
    {
        foreach (Vector3 v in positionArray)
        {
            int x, y;
            GetXY(v, out x, out y);

            GameObject cube = GameObject.Instantiate(cubeGO);
            cube.transform.localScale *= cellSize;
            cube.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f;

            // Setting up the gradient color
            float time = GetValue(x, y) / HEAT_MAP_MAX_VALUE;
            Color gradientColor = gradient.Evaluate(time);

            cube.GetComponent<Renderer>().material.color = gradientColor;
        }
    }
}