using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameManagerState currentState { get; private set; } = GameManagerState.Idle;
    private List<GameButton> playSequnce = new List<GameButton>();
    public static Action<int> PlayerDidWin;
    
    private SingleGameConfiguration levelConfiguration = LevelConfigurationHolder.Configuration;
    
    private int score;
    [SerializeField] private TMP_Text scoreText;
    private float countdownTime;
    [SerializeField] private TMP_Text countdownText;
    private Coroutine countdownCoroutine;
    [SerializeField] private EndOfGameWindow endOfGameWindow;
    private int playerCurPlaySequenceListIndex;
    private float baseDelayBetweenLightUp = 0.5f;
    
    [SerializeField] private GameObject board;

    [SerializeField] private GameButton gameButtonPrefab;
    [SerializeField] private GameButton [] gameButtons;
    
    [SerializeField] private Color[] gameButtonsColors;
    
    [SerializeField] private AudioClip[] gameButtonsSounds;

    void Start()
    {
        InitiateBoard();
        InitiateTime();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();   
    }
    
    private void Subscribe()
    {
        PreGameWindow.PlayerDidPressStart += OnPlayerDidPressStart;
        LeaderBoard.PlayerClosedLeaderBoard += OnPlayerClosedLeaderBoard;

    }
    
    private void Unsubscribe()
    {
        PreGameWindow.PlayerDidPressStart -= OnPlayerDidPressStart;
        LeaderBoard.PlayerClosedLeaderBoard -= OnPlayerClosedLeaderBoard;
    }

    private void OnPlayerDidPressStart()
    {
        ChangeState(GameManagerState.SequencePlay);
    }
    private void OnPlayerClosedLeaderBoard()
    {
        endOfGameWindow.message.text = "Well Done!";
        endOfGameWindow.Show();
    }
    
    private void InitiateTime()
    {
        countdownTime = levelConfiguration.gameTime;
        countdownText.text = $"Time: {countdownTime}";
    }
    private void InitiateBoard()
    {
        gameButtons = new GameButton[levelConfiguration.numOfGameButtons];

        for (int i = 0; i < gameButtons.Length; i++)
        {
            GameButton newGameButton = Instantiate(gameButtonPrefab, board.transform);
            
            newGameButton.buttonComponent.onClick.AddListener(() => OnGameButtonClick(newGameButton));
            newGameButton.buttonComponent.image.color = gameButtonsColors[i];
            newGameButton.buttonSound.clip = gameButtonsSounds[i];

            gameButtons[i] = newGameButton;
        }
    }
    
    private IEnumerator CountdownRoutine()
    {
        while (countdownTime > 0)
        {
            countdownTime -= 1.0f;
            countdownText.text = $"Time: {countdownTime}";
            yield return new WaitForSeconds(1.0f);
        }
        Debug.Log("before change to Win");
        ChangeState(GameManagerState.Win);
    }
    
    private void OnGameButtonClick(GameButton gameButton)
    {
        if (currentState == GameManagerState.PlayerTurn)
        {
            StartCoroutine(PlayerButtonClickRoutine(gameButton));
        }
    }

    private IEnumerator PlayerButtonClickRoutine(GameButton gameButton)
    {
        yield return new WaitForSeconds(0.2f);
        
        gameButton.ButtonIsPressed();
        gameButton.buttonSound.Play();

        score += levelConfiguration.pointsPerStep;
        scoreText.text = $"score: {score}";
        
        yield return new WaitForSeconds(0.2f);
        gameButton.ButtonIsReleased();
        
        //if button not match the game has ended
        if (gameButton != playSequnce[playerCurPlaySequenceListIndex])
        {
            ChangeState(GameManagerState.Lose);
        }
        else
        {
            playerCurPlaySequenceListIndex++;
            if (playerCurPlaySequenceListIndex >= playSequnce.Count)
            {
                ChangeState(GameManagerState.SequencePlay);
            }
        }
    }

    private void PlaySequence()
    {
        int randomButtonIndex = Random.Range(0, gameButtons.Length);
        GameButton randomButton = gameButtons[randomButtonIndex];
        playSequnce.Add(randomButton);

        StartCoroutine(SequencePlayRoutine());
    }
    
    private IEnumerator SequencePlayRoutine()
    {
        int startIndex = 0;
        if (!levelConfiguration.repeatMode)
        {
            startIndex = playSequnce.Count - 1;
        }
        
        float delay = baseDelayBetweenLightUp / levelConfiguration.gameSpeed;
        
        for (int i = startIndex; i < playSequnce.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            
            GameButton gameButton = playSequnce[i];
            
            yield return new WaitForSeconds(0.2f);
        
            gameButton.ButtonIsPressed();
            gameButton.buttonSound.Play();

            yield return new WaitForSeconds(0.2f);
            gameButton.ButtonIsReleased();
        }
        
        ChangeState(GameManagerState.PlayerTurn);
    }

    private bool ChangeState(GameManagerState newState)
    {
        bool didChange = false;
        switch (currentState)
        {
            case GameManagerState.Idle:
                if (newState != GameManagerState.SequencePlay) break;
                countdownCoroutine = StartCoroutine(CountdownRoutine());
                currentState = newState;
                didChange = true;
                break;
            
            case GameManagerState.SequencePlay:
                if (newState != GameManagerState.Win &&
                    newState != GameManagerState.PlayerTurn) break;
                currentState = newState;
                didChange = true;
                break;
            
            case GameManagerState.PlayerTurn:
                if (newState != GameManagerState.Win &&
                    newState != GameManagerState.Lose &&
                    newState != GameManagerState.SequencePlay) break;
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
            case GameManagerState.SequencePlay:
                PlaySequence();
                break;
            case GameManagerState.PlayerTurn:
                playerCurPlaySequenceListIndex = 0;
                break;
            case GameManagerState.Win:
                Debug.Log("Beforee Invkoe");
                PlayerDidWin?.Invoke(score);
                Debug.Log("AfterInovoke");
                break;
            case GameManagerState.Lose:
                StopCoroutine(countdownCoroutine);
                endOfGameWindow.message.text = "Something was missing...";
                endOfGameWindow.Show();
                break;
        }
    }
    
    public enum GameManagerState 
    {
        Idle,
        SequencePlay,
        PlayerTurn,
        Win,
        Lose
    }
}
