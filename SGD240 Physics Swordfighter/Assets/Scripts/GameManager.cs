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
    [SerializeField] private GameObject hearts;
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private UIHeart[] player1Hearts;
    [SerializeField] private UIHeart[] player2Hearts;

    private void Awake()
    {
        TriggerChecker.OnPlayerDamage += DamagePlayer; // Connect to both player's TriggerCheckers
        CurrentGameState = GameState.PreGame;

        player1Health = maxHealth;
        player2Health = maxHealth;

        Time.timeScale = 0.8f;
    }

    private void Start()
    {
        startingScreen.SetActive(true);
        hearts.SetActive(false);
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

            foreach (UIHeart heartIcon in player1Hearts) // Checks health UI, modular setup if max health is increased or decreased, also if multiple hearts are lost at once
            {
                if (heartIcon.healthThreshold > player1Health)
                {
                    heartIcon.PopHeart();
                }

                else
                {
                    continue;
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

            foreach (UIHeart heartIcon in player2Hearts)
            {
                if (heartIcon.healthThreshold > player2Health)
                {
                    heartIcon.PopHeart();
                }

                else
                {
                    continue;
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
            Time.timeScale = 0.1f;
        }
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Playing;
        startingScreen.SetActive(false);
        hearts.SetActive(true);
    }

    private void OnDestroy() // Unsubscribe to damageplayer event!
    {
        TriggerChecker.OnPlayerDamage -= DamagePlayer;
    }
}
