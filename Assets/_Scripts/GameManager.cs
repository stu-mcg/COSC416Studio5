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
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
        currentLives = maxLives;
        HUDManager.Instance.UpdateLivesDisplay(currentLives);
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public ParticleSystem brickDestructionEffect;


    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        // implement particle effect here
        Instantiate(brickDestructionEffect, position, Quaternion.identity);
        // add camera shake here
        currentBrickCount--;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        if(currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        currentLives--;
        HUDManager.Instance.UpdateLivesDisplay(currentLives);
        // update lives on HUD here
        // game over UI if currentLives <= 0, then exit to main menu after delay
        if (currentLives <= 0)
        {
            currentLives = 0;
            GameOver();
        }
        else
        {
            ball.ResetBall();
        }
    }
    private void GameOver()
    {
        Time.timeScale = 0; // Freeze time
        HUDManager.Instance.ShowGameOverScreen(); // Show game-over screen
        StartCoroutine(ReturnToMainMenuAfterDelay());
    }
    private IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSecondsRealtime(1.5f); // Wait in real-time (ignoring Time.timeScale)
        Time.timeScale = 1; // Reset time scale
        SceneHandler.Instance.LoadMenuScene(); // Load main menu
    }
}
