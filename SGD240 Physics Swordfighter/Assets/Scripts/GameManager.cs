using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private int maxHealth;

    private int player1Health;
    private int player2Health;

    [Header("Game State")]
    public int winningPlayer;
    public enum GameState { PreGame, Playing, EndGame }
    public static GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        TriggerChecker.OnPlayerDamage += DamagePlayer; // Connect to both player's TriggerCheckers
        CurrentGameState = GameState.PreGame;

        player1Health = maxHealth;
        player2Health = maxHealth;
    }

    private void DamagePlayer(int playerNumber)
    {
         if (playerNumber == 1)
         {
            if (player1Health > 0)
            {
                player1Health -= 1;

                if (player1Health <= 0)
                {
                    winningPlayer = 2;
                }
            }
         }

         if (playerNumber == 2)
         {
            if (player2Health > 0)
            {
                player2Health -= 1;

                if (player2Health <= 0)
                {
                    winningPlayer = 1;
                }
            }
         }

        VictorCheck();
    }

    private void VictorCheck()
    {
        if (player1Health == 0 || player2Health == 0)
        {
            CurrentGameState = GameState.EndGame;
        }
    }
}
