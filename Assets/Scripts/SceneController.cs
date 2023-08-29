using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SceneController : MonoBehaviour
{
    public bool isRunning;
    [SerializeField] private GameObject[] SpawnableEnemies;
    [SerializeField] private float actionCooldown = 3f;
    [SerializeField] private int maxEnemiesAliveAtOnce = 10;
    private int _currentEnemyCount;
    private Random _random;
    private GameObject[] _enemySpawners;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float elapsed = 0f;
    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;
        elapsed += Time.deltaTime;
        if (elapsed >= actionCooldown && _currentEnemyCount < maxEnemiesAliveAtOnce)
        {
            elapsed %= actionCooldown;
            
            // Every time our actionCooldown time has passed, spawn a new enemy if enemies can be spawned
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Choose an enemy to spawn, spawn them
        int newEnemyIndex = _random.NextInt(0, SpawnableEnemies.Length);
        GameObject enemyPrefab = SpawnableEnemies[newEnemyIndex];
        GameObject newEnemy = Instantiate(enemyPrefab);
        
        // Choose a position for the newly spawned enemy
        // Choose a random spawner and then spawn at a random position at that spawners x axis
        int enemySpawnerIndex = _random.NextInt(0, _enemySpawners.Length);
        GameObject spawner = _enemySpawners[enemySpawnerIndex];
        float enemyXAxis = _random.NextFloat(-5.9f, 6f);
        Vector3 enemyPosition = new Vector3(enemyXAxis, spawner.transform.position.y, 0f);
        newEnemy.transform.position = enemyPosition;

        // Increment our enemy count
        _currentEnemyCount += 1;
    }

    public void KillEnemy()
    {
        _currentEnemyCount -= 1;
    }
    
    public void StartScene()
    {
        _currentEnemyCount = 0;
        _enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        // TODO: Need seed rotation or something here
        _random = new Random(1);
        isRunning = true;
    }
}
