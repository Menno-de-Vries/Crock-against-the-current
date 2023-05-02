using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenSpawning;

    [SerializeField]
    private ObjectPool _logPool;

    [SerializeField]
    private ObjectPool _plantPool;

    public ObjectPool PickupPool;

    private Paths[] _paths;

    private int _amountOfPaths = 3;

    public static LevelGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        _paths = GameManager.Instance.Paths;

        //starts the coroutine for spawning obstacles
        StartCoroutine(SpawnObstacle());
    }

    /// <summary>
    /// Spawns the obstacles
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnObstacle()
    {
        while (true)
        {
            //waits a certain amount of time
            yield return new WaitForSeconds(_timeBetweenSpawning);

            int amountOfObstacles = Random.Range(2, _amountOfPaths + 1);

            //for the amount of obstacles it spawns an obstacle
            for (int i = 0; i < amountOfObstacles; i++)
            {
                int thisPath = Random.Range(0, _amountOfPaths);

                //the path is not full it will spawn an obstacle
                if (_paths[thisPath].IsPathFull == false)
                {
                    if (RandomChance(3))
                    {
                        _plantPool.GetPooledObject(transform.position + new Vector3(_paths[thisPath].PathPositionX, 0, 0), Quaternion.identity);
                    }
                    else
                    {
                        _logPool.GetPooledObject(transform.position + new Vector3(_paths[thisPath].PathPositionX, 0, 0), Quaternion.identity);
                        if (RandomChance(4))
                        {
                            PickupPool.GetPooledObject(transform.position + new Vector3(_paths[thisPath].PathPositionX, PickupPool.PooledObject.transform.position.y, 0), Quaternion.identity);
                        }
                    }
                    _paths[thisPath].IsPathFull = true;
                }
            }

            //sets all the paths empty
            for (int i = 0; i < _amountOfPaths; i++)
            {
                _paths[i].IsPathFull = false;
            }
        }
    }

    private bool RandomChance(int maxValue)
    {
        int random = Random.Range(0, maxValue);

        if (random == maxValue - 1)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ObstacleTag01>())
        {
            _logPool.ReturnPooledObject(other.GetComponent<PoolItem>());
        }
        else if (other.gameObject.GetComponent<ObstacleTag01>())
        {
            _plantPool.ReturnPooledObject(other.GetComponent<PoolItem>());
        }
        else if (other.gameObject.GetComponent<PickupTag>())
        {
            PickupPool.ReturnPooledObject(other.GetComponent<PoolItem>());
        }
    }
}