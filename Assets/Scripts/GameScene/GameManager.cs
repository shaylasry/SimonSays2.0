using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameManagerState currentState { get; private set; } = GameManagerState.Idle;
    public static Action<int> PlayerDidWin;
    public static Action<GameButton[]> GameDidStart;

    //Level configuration instance
    private SingleGameConfiguration levelConfiguration = LevelConfigurationHolder.Configuration;
    
    //Game data instances
    [SerializeField] private EndOfGameWindow endOfGameWindow;
    private int score;
    
    //Play sequence state instances
    private int playerCurPlaySequenceListIndex;
    
    //Game objects instances
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject board;
    [SerializeField] private GameButton gameButtonPrefab;
    [SerializeField] private GameButton [] gameButtons;
    [SerializeField] private GameButtonsData gameButtonsData;

    void Start()
    {
        InitiateBoard();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();   
    }
    
    //Use c# event so each game object can handle it's own functionality and we can still manage passing between Game scene states
    private void Subscribe()
    {
        PreGameWindow.PlayerDidPressStart += OnPlayerDidPressStart;
        LeaderBoard.PlayerClosedLeaderBoard += OnPlayerClosedLeaderBoard;
        GameController.SendPlayerScore += OnGameControllerSentScore;
    }
    
    private void Unsubscribe()
    {
        PreGameWindow.PlayerDidPressStart -= OnPlayerDidPressStart;
        LeaderBoard.PlayerClosedLeaderBoard -= OnPlayerClosedLeaderBoard;
        GameController.SendPlayerScore -= OnGameControllerSentScore;
    }

    private void OnGameControllerSentScore(int playerScore)
    {
        if (playerScore < 0)
        {
            ChangeState(GameManagerState.Lose);
        }
        else
        {
            score = playerScore;
            ChangeState(GameManagerState.Win);
        }
    }

    private void OnPlayerDidPressStart()
    {
        ChangeState(GameManagerState.Running);
    }
    private void OnPlayerClosedLeaderBoard()
    {
        endOfGameWindow.message.text = "Well Done!";
        endOfGameWindow.Show();
    }
    
    private void InitiateBoard()
    {
        gameButtons = new GameButton[levelConfiguration.numOfGameButtons];

        for (int i = 0; i < gameButtons.Length; i++)
        {
            GameButton newGameButton = Instantiate(gameButtonPrefab, board.transform);
            
            newGameButton.buttonComponent.onClick.AddListener(() => OnGameButtonClick(newGameButton));
            newGameButton.buttonComponent.image.color = gameButtonsData.gameButtonsColors[i];
            newGameButton.buttonSound.clip = gameButtonsData.gameButtonsSounds[i];

            gameButtons[i] = newGameButton;
        }
    }

    private void OnGameButtonClick(GameButton gameButton)
    {
        //Player can't push button when it's not his turn to play
        if (gameController.currentState != GameController.GameState.PlayerTurn) return;
        gameController.PlayerClickedOnGameButton(gameButton);
    }

    private bool ChangeState(GameManagerState newState)
    {
        bool didChange = false;
        switch (currentState)
        {
            case GameManagerState.Idle:
                if (newState != GameManagerState.Running) break;
                currentState = newState;
                didChange = true;
                break;
            
            case GameManagerState.Running:
                if (newState != GameManagerState.Win &&
                    newState != GameManagerState.Lose) break;
                currentState = newState;
                didChange = true;
                break;
        }
        
        if (didChange) OnStateChange();
        
        return didChange;
    }
    
    private void OnStateChange()
    {
        switch (currentState)
        {
            case GameManagerState.Idle:
                break;
            case GameManagerState.Running:
                GameDidStart?.Invoke(gameButtons);
                break;
            case GameManagerState.Win:
                PlayerDidWin?.Invoke(score);
                break;
            case GameManagerState.Lose:
                endOfGameWindow.message.text = "Something was missing...";
                endOfGameWindow.Show();
                break;
        }
    }
    
    public enum GameManagerState 
    {
        Idle,
        Running,
        Win,
        Lose
    }
}
