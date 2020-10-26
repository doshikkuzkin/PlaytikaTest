using UnityEngine;

public class Tile : MonoBehaviour
{
    private readonly float instantiateOffset = 5f;
    private GameObject obstacle;
    private bool obstaclePassed;

    private void Update()
    {
        //update score if player pass obstacle
        if (transform.position.z < 0 && !obstaclePassed)
            if (obstacle != null)
            {
                PlayerEvents.FireScoreUp(5, true);
                obstaclePassed = true;
            }
    }

    //move tile
    public void Move(float speed)
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    public float GetZPosition()
    {
        return transform.position.z;
    }

    //spawb obstacle with rnadom offset
    public void SpawnObstacle(GameObject obstacle)
    {
        if (this.obstacle == null)
        {
            var currentObj = Instantiate(obstacle, transform);
            currentObj.transform.localPosition = new Vector3(Random.Range(-instantiateOffset, instantiateOffset), 2, 0);
            this.obstacle = currentObj;
            obstaclePassed = false;
        }
    }

    //destroy current obstacle
    public void DestroyObstacle()
    {
        if (obstacle != null)
        {
            Destroy(obstacle.gameObject);
            obstacle = null;
        }
    }
}