using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 30f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += new Vector3(h, 0, v) * speed * Time.deltaTime;
    }
}