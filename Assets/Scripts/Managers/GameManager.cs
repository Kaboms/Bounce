using CoreFeatures.Singleton;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent GameOverEvent = new UnityEvent();

    public bool IsGameOver = false;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player && player.TryGetComponent(out BallController ballController))
        {
            ballController.DeathEvent.AddListener(GameOver);
        }
    }

    private void GameOver()
    {
        IsGameOver = true;
        GameOverEvent.Invoke();
    }
}
