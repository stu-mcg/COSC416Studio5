using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : SingletonMonoBehavior<HUDManager>
{
    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI livesText; // Text UI element to display lives
    [SerializeField] private GameObject gameOverScreen; // Game Over UI screen

    private void Start()
    {
        UpdateLivesDisplay(GameManager.Instance.MaxLives);
        gameOverScreen.SetActive(false); // Ensure the game-over screen is hidden initially
    }

    public void UpdateLivesDisplay(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true); // Show the game-over screen
    }
}