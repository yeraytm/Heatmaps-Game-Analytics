using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    // bool limitedMaxValue;
    // other customizable variables...

    public GameObject playerGO;
    public GameObject cubeGO;

    private Heatmap grid;

    public Gradient gradient = new Gradient
    {
        alphaKeys = new[]
        {
            new GradientAlphaKey(0.5f, 0f),
            new GradientAlphaKey(0.5f, 1f)
        },

        colorKeys = new[]
        {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.green, 0.5f),
            new GradientColorKey(Color.blue, 1f)
        }
    };

    List<SpatialData> spatialDatas;

    private void Start()
    {
        grid = new Heatmap(44, 32, 2.5f, new Vector3(-35,0,-40), gradient, spatialDatas, cubeGO);
    }

    private void Update()
    {
        
    }
}
