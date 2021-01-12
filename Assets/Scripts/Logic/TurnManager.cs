
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // for Singleton Pattern
    public static TurnManager Instance;
    
    public bool gameStarted = false;

    public Enemy[] enemies;

    private int TurnCount { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        enemies = FindObjectsOfType<Enemy>();
    }

    private void Start()
    {
    	OnGameStart();
    }

    private void OnGameStart()
    {
        Card.CardsCreated.Clear();

        HoverPreview.PreviewsAllowed = false;

        gameStarted = true;
    }

    public void EndTurn()
    {
        TurnCount++;

        /*if (isEnemyTurn)
        {
            isEnemyTurn = false;

            foreach(var e in Enemies)
            {
                e.OnTurnEnd();
            }

            CurrentPlayer.GetComponent<PlayerTurnMaker>().OnTurnStart();
            
        }
        else
        {
            // send all commands in the end of current player`s turn
            CurrentPlayer.OnTurnEnd();

            GlobalSettings.Instance.EndTurnButton.interactable = false;

            isEnemyTurn = true;

            enemyTurnCount++;
            new StartEnemyTurnCommand().AddToQueue();
        }*/
    }

    /*public void GameWon()
    {
        //CurrentPlayer.DiscardHand();

        GlobalSettings.Instance.EndTurnButton.gameObject.SetActive(false);
        new DelayCommand(1f).AddToQueue();
        new EndofFightCommand(true).AddToQueue();
    }

    public void GameLost()
    {
        new ShowMessageCommand("Defeat!", 3f);
    }*/
}