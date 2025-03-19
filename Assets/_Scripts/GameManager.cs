using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;

    private int currentBrickCount;
    private int totalBrickCount;
    private int currentLives;

    public int MaxLives => maxLives;

    private void OnEnable()
    {
        // Add listener for fire input if available.
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnFire.AddListener(FireBall);
        }
        else
        {
            Debug.LogError("InputHandler.Instance is null!");
        }

        // Reset the ball if reference is set.
        if (ball != null)
        {
            ball.ResetBall();
        }
        else
        {
            Debug.LogError("Ball reference is not set in GameManager!");
        }

        // Check bricksContainer before using its childCount.
        if (bricksContainer != null)
        {
            totalBrickCount = bricksContainer.childCount;
            currentBrickCount = bricksContainer.childCount;
        }
        else
        {
            Debug.LogError("BricksContainer is not set in GameManager!");
            totalBrickCount = 0;
            currentBrickCount = 0;
        }

        // Explicitly set currentLives to maxLives (3).
        currentLives = maxLives;

        // Update HUD lives display if HUDManager exists.
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateLivesDisplay(currentLives);
        }
        else
        {
            Debug.LogError("HUDManager instance not found!");
        }
    }

    private void OnDisable()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.OnFire.RemoveListener(FireBall);
        }
    }

    private void FireBall()
    {
        if (ball != null)
        {
            ball.FireBall();
        }
    }

    public ParticleSystem brickDestructionEffect;

    public void OnBrickDestroyed(Vector3 position)
    {
        // Fire audio and instantiate particle effect.
        if (brickDestructionEffect != null)
        {
            Instantiate(brickDestructionEffect, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("brickDestructionEffect is not set in GameManager!");
        }

        // Add camera shake here if needed.
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");

        if (currentBrickCount == 0)
        {
            SceneHandler.Instance.LoadNextScene();
        }
    }

    public void KillBall()
    {
        currentLives--;
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateLivesDisplay(currentLives);
        }

        // Check for game over condition.
        if (currentLives <= 0)
        {
            currentLives = 0;
            GameOver();
        }
        else
        {
            if (ball != null)
            {
                ball.ResetBall();
            }
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0; // Freeze time.
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.ShowGameOverScreen();
        }
        StartCoroutine(ReturnToMainMenuAfterDelay());
    }

    private IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f); // Wait in real-time.
        Time.timeScale = 1; // Reset time scale.
        SceneHandler.Instance.LoadMenuScene();
    }
}
