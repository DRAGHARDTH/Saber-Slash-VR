using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class GameManager : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 120f; // Game duration in seconds


    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText; // Text element for displaying the timer
    [SerializeField] private TextMeshProUGUI scoreText; // Text element for displaying the score
    [SerializeField] private GameObject intro;
    [SerializeField] private GameObject scoreCard;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private TextMeshProUGUI finalScoreCardText;

    [Header("Game State")]
    public float ElapsedTime { get; private set; } = 0;
    private float timer; // Remaining time for the game
    public bool isGameActive = false; // Tracks if the game is currently active
    private bool isStartDelayActive = false; // Tracks if the start delay is active
    private int score = 0; // Player's current score

    public void StartGame()
    {
        if (!isStartDelayActive)
        {
            isStartDelayActive = true; // Prevents multiple coroutine calls
            StartCoroutine(StartGameAfterDelay(10f)); // Starts the game after 5 seconds
        }
    }

    IEnumerator StartGameAfterDelay(float delay)
    {
        Debug.Log("Game starting in " + delay + " seconds...");
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        intro.SetActive(false);
        scoreCard.SetActive(true);
        isGameActive = true;
        isStartDelayActive = false;
        
        // Initialize game state
        timer = gameDuration;
        score = 0;
        UpdateScore(0); // Display initial score
        UpdateTimer(); // Display initial timer

        Debug.Log("Game Started!");
    }
    void Update()
    {
        // Update the timer only if the game is active
        if (isGameActive)
        {
            timer -= Time.deltaTime; // Decrease the timer
            UpdateTimer(); // Update the timer UI
            ElapsedTime += Time.deltaTime;
            // Check if time has run out
            if (timer <= 0)
            {
                EndGame();
            }
        }
    }


    /// <summary>
    /// Starts the game by activating the game state and hiding the start button.
    /// </summary>
    

    /// <summary>
    /// Ends the game, stops the game loop, and displays the final score.
    /// </summary>
    public void EndGame()
    {
        isGameActive = false; // Stop the game
        scoreCard.SetActive(false);
        endMenu.SetActive(true);
        
        finalScoreCardText.text = "Game Over! Final Score: " + score;
        
        ElapsedTime = 0;

    }
    private void UpdateTimer()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timer); // Display time rounded to nearest second
    }

    public void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score; // Update score display
    }
    public void ExitGame()
    {
        // Log exit action in the Unity editor
        Debug.Log("Exit Game");
        // Quit the application (won't work in the editor, only in builds)
        Application.Quit();
    }
}


