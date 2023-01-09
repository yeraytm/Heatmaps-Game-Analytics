using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicMovement : MonoBehaviour
{
    private float speed = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        float zMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1

        gameObject.transform.position += new Vector3(xMove, 0, zMove) * speed * Time.deltaTime;
    }
}
