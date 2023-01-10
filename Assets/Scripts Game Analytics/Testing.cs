using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
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

    private void Start()
    {
        grid = new Heatmap(44, 32, 2.5f, new Vector3(-35,0,-40), gradient, cubeGO);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Increment cell value
            grid.IncrementValue(playerGO.transform.position);
        }
    }
}
