using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float baseSpeed = 5f; // Initial speed of the block
    [SerializeField] private float currentSpeed;// Current speed of the block, adjusted dynamically
    [SerializeField] private Transform playerTransform; // Reference to the player's position

    private void Start()
    {
        // Initialize the block's speed
        currentSpeed = baseSpeed;

        // Find and store the player's transform
        playerTransform = GameObject.Find("Player").transform;
    }
    
    // Set the GameManager reference dynamically.
    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    private void Update()
    {
        // Exit if the game is not active
        if (!gameManager.isGameActive) return;

        // Increase speed dynamically based on the game's elapsed time
        currentSpeed = baseSpeed + (gameManager.ElapsedTime / 20f);

        // Move the block towards the player's position
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);

        // Check if the block has moved past the player
        if (transform.position.z <= playerTransform.position.z)
        {
            gameManager.UpdateScore(-5); // Deduct points for missing the block
            gameObject.SetActive(false);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Sword")) // Check if the object colliding is tagged "Sword"
        {
            // Add points for hitting the block and destroy it
            gameManager.UpdateScore(10);
           
        }
    }
    
    
    
}
