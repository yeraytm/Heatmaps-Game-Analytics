using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    [SerializeField]
    public GameObject playerGO;

    private Grid grid;

    private void Start()
    {
        grid = new Grid(44, 32, 2.5f, new Vector3(-35,0,-40));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetValue(playerGO.transform.position, 56);
        }
    }
}
