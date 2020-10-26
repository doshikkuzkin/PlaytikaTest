using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 3f;
    private float direction;
    private readonly float[] directions = {-1, 1};

    private void Start()
    {
        //choose rotation direction
        direction = directions[Random.Range(0, directions.Length)];
    }

    private void Update()
    {
        //rotate object around Y axis
        transform.Rotate(new Vector3(0f, direction * speed, 0f) * Time.deltaTime);
    }
}