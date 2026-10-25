using TMPro;
using UnityEngine;

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

    [Header("Interface")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject startingScreen;
    [SerializeField] private TextMeshProUGUI endingText;

    private void Awake()
    {
        TriggerChecker.OnPlayerDamage += DamagePlayer; // Connect to both player's TriggerCheckers
        CurrentGameState = GameState.PreGame;

        player1Health = maxHealth;
        player2Health = maxHealth;
    }

    private void Start()
    {
        startingScreen.SetActive(true);
        endScreen.SetActive(false);
    }

    private void DamagePlayer(int playerNumber) // Subtracts player health, then checks if either player has won yet
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
        if (CurrentGameState == GameState.EndGame) // Stop repeated logic
        {
            return;
        }

        if (player1Health == 0 || player2Health == 0)
        {
            CurrentGameState = GameState.EndGame;
            endScreen.SetActive(true);
            endingText.SetText("player " + winningPlayer + " wins!");
        }
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        startingScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        TriggerChecker.OnPlayerDamage -= DamagePlayer;
    }
}
