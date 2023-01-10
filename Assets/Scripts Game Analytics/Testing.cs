using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject cubeGO;

    private Grid grid;

    private void Start()
    {
        grid = new Grid(44, 32, 2.5f, new Vector3(-35,0,-40), cubeGO);
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
