using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private float tilesSpeed;

    [SerializeField] private float normalTileSpeed = 8f;

    public GameObject tile;
    public GameObject asteroid;
    private Transform lastTile;
    private readonly float maxTilesSpeed = 80f;
    private readonly float minSpawnRate = 0.5f;

    private bool moving;

    private float obstacleSpawnRate = 3f;
    private float obstacleSpawnTime;
    private readonly float tileDestroyCoordinate = -14f;
    private readonly float tileHeigth = -2.9f;
    private readonly float tileLenght = 14f;

    private List<Tile> tiles;
    private readonly float tileSpeedBoost = 3f;

    private int tilesSpawned;
    private float timeBoostSpeed;

    private float timeNormalSpeed;


    private void Awake()
    {
        tilesSpeed = normalTileSpeed;
    }

    private void Start()
    {
        tiles = new List<Tile>();
        var initialTiles = GetComponentsInChildren<Tile>();
        tiles.AddRange(initialTiles);
        lastTile = tiles[tiles.Count - 1].transform;
        SpawnInitialObstacles();
        //stop moving tiles if player loose
        PlayerEvents.PlayerDeath += StopMoving;
    }

    private void Update()
    {
        if (moving)
        {
            //Count flying time with/without boost
            if (IsBoostedSpeed())
            {
                timeBoostSpeed += Time.deltaTime;
            }
            else
            {
                timeBoostSpeed = 0;
                timeNormalSpeed += Time.deltaTime;
            }

            //Update score after one second
            if (timeBoostSpeed >= 1)
            {
                timeBoostSpeed = 0;
                //fire score event
                PlayerEvents.FireScoreUp(2, false);
            }

            if (timeNormalSpeed >= 1)
            {
                timeNormalSpeed = 0;
                PlayerEvents.FireScoreUp(1, false);
            }

            //Move all tiles in List
            for (var i = 0; i < tiles.Count; i++)
            {
                tiles[i].Move(tilesSpeed);

                //if player pass tile, move it to the end of list
                if (tiles[i].GetZPosition() < tileDestroyCoordinate)
                {
                    tiles[i].transform.position = new Vector3(0, tileHeigth, lastTile.position.z + tileLenght);
                    tiles[i].DestroyObstacle();
                    lastTile = tiles[i].transform;
                    var currentTile = tiles[i];
                    tiles.Remove(tiles[i]);
                    tiles.Add(currentTile);
                    tilesSpawned++;
                }
            }

            //Spawn Tile if enough time passed
            obstacleSpawnTime += Time.deltaTime;
            if (obstacleSpawnTime >= obstacleSpawnRate)
            {
                obstacleSpawnTime = 0;
                lastTile.GetComponent<Tile>().SpawnObstacle(asteroid);
            }

            //Change difficulty every 20 tiles
            if (tilesSpawned > 20)
            {
                tilesSpawned = 0;
                IncreaseDifficulty();
            }
        }
    }

    //enble or disable tiles movement
    public void IsMovingTiles(bool isMoving)
    {
        moving = isMoving;
    }

    private void StopMoving()
    {
        IsMovingTiles(false);
    }

    private void SpawnInitialObstacles()
    {
        var j = 0;
        for (var i = 0; i < tiles.Count; i++, j++)
            if (j == 6)
            {
                tiles[i].SpawnObstacle(asteroid);
                j = 0;
            }
    }

    private void IncreaseDifficulty()
    {
        if (tilesSpeed < maxTilesSpeed) normalTileSpeed += 6;
        if (obstacleSpawnRate > minSpawnRate) obstacleSpawnRate -= 0.5f;
    }

    private bool IsBoostedSpeed()
    {
        return normalTileSpeed != tilesSpeed;
    }

    public void BoostTileSpeed()
    {
        tilesSpeed = normalTileSpeed * tileSpeedBoost;
    }

    public void RemoveBoostSpeed()
    {
        tilesSpeed = normalTileSpeed;
    }
}