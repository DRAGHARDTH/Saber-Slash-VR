using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameManager gameManager;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints; // Array of 4 spawn points
    [SerializeField] private float initialSpawnRate = 1f; // Spawn every second
    [SerializeField] private float spawnRateIncreaseInterval = 15f; // Time to increase spawn rate
    [SerializeField] private float speedIncreaseFactor = 0.03f;
    [SerializeField] private int poolSize = 10; // Size of the pool

    private float spawnRate;
    private float nextSpawnTime;
    private float gameTimeElapsed;

    private GameObject[] blockPool;
    private int currentBlockIndex = 0;

    private void Start()
    {
        spawnRate = initialSpawnRate;
        nextSpawnTime = Time.time + spawnRate;

        // Initialize the block pool
        InitializeBlockPool();
    }

    private void InitializeBlockPool()
    {
        blockPool = new GameObject[poolSize];

        // Populate the pool with inactive blocks
        for (int i = 0; i < poolSize; i++)
        {
            blockPool[i] = Instantiate(blockPrefab);
            blockPool[i].SetActive(false); // Deactivate the block initially
        }
    }


    private void Update()
    {
        if (!gameManager.isGameActive) return;

        if (gameManager.ElapsedTime > 118) return;
        
        gameTimeElapsed += Time.deltaTime;
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnBlock();
            nextSpawnTime = Time.time + spawnRate;
        }
        if (gameTimeElapsed >= spawnRateIncreaseInterval)
        {
            gameTimeElapsed = 0;
           
            spawnRate = Mathf.Max(0.3f, spawnRate - speedIncreaseFactor); // Cap spawn rate
            Debug.Log("Spawn rate increased! New spawn rate: " + spawnRate + " seconds per block.");
        }
       

        if (gameManager.ElapsedTime >= nextSpawnTime)
        {
            SpawnBlock();
            nextSpawnTime = Time.time + spawnRate;
        }

        
    }

    private void SpawnBlock()
    {
        // Get a block from the pool
        GameObject block = GetPooledBlock();

        if (block != null)
        {
            // Choose a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Activate the block and set its position
            block.transform.position = spawnPoint.position;
            block.transform.rotation = spawnPoint.rotation;
            block.SetActive(true);

            // Initialize the block script
            Block blockScript = block.GetComponent<Block>();
            if (blockScript != null)
            {
                blockScript.SetGameManager(gameManager); // Pass the GameManager reference to the Block script
            }
        }


    }
    private GameObject GetPooledBlock()
    {
        // Loop through the pool and return the first inactive block
        for (int i = 0; i < poolSize; i++)
        {
            currentBlockIndex = (currentBlockIndex + 1) % poolSize;
            if (!blockPool[currentBlockIndex].activeInHierarchy)
            {
                return blockPool[currentBlockIndex];
            }
        }

        // If no inactive blocks, return null
        Debug.LogWarning("No available blocks in the pool");
        return null;
    }

}
