using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static Action<int> SendPlayerScore;
    public GameState currentState { get; private set; } = GameState.Idle;
    
    //Level configuration instance
    private SingleGameConfiguration levelConfiguration = LevelConfigurationHolder.Configuration;

    //Game data instances
    private bool didPlayerWin = false;
    private int score;
    private float baseDelayBetweenLightUp = 0.5f;
    [SerializeField] private TMP_Text scoreText;
    private float countdownTime;
    [SerializeField] private TMP_Text countdownText;
    private Coroutine countdownCoroutine;

    //Play sequence state instances
    private int playerCurPlaySequenceListIndex;
    private List<GameButton> playSequnce = new List<GameButton>();
    
    //Game objects instances
    [SerializeField] private GameButton [] gameButtons;
    [SerializeField] private AudioClip wrongButtonSound;
    void Start()
    {
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
    
    //Use c# event so each game object can handle it's own functionality and we can still manage passing between Game scene states
    private void Subscribe()
    {
        GameManager.GameDidStart += OnGameDidStart;
    }

    private void Unsubscribe()
    {
        GameManager.GameDidStart += OnGameDidStart;
    }

    private void OnGameDidStart(GameButton[] buttons)
    {
        gameButtons = buttons;
        ChangeState(GameState.SequencePlay);
    }


    private void InitiateTime()
    {
        countdownTime = levelConfiguration.gameTime;
        countdownText.text = $"Time: {countdownTime}";
    }
    
    private IEnumerator CountdownRoutine()
    {
        while (countdownTime > 0)
        {
            countdownTime -= 1.0f;
            countdownText.text = $"Time: {countdownTime}";
            yield return new WaitForSeconds(1.0f);
        }

        didPlayerWin = true;
        ChangeState(GameState.End);
    }
    
    public void PlayerClickedOnGameButton(GameButton gameButton)
    {
        bool isValidButton = gameButton == playSequnce[playerCurPlaySequenceListIndex];
        
        if (!isValidButton)
        {
            gameButton.buttonSound.clip = wrongButtonSound;
        }
        
        gameButton.buttonSound.Play();

        score += levelConfiguration.pointsPerStep;
        scoreText.text = $"score: {score}";
        
        //if button not match the game has ended
        if (!isValidButton)
        {
            ChangeState(GameState.End);
        }
        else
        {
            playerCurPlaySequenceListIndex++;
            if (playerCurPlaySequenceListIndex >= playSequnce.Count)
            {
                ChangeState(GameState.SequencePlay);
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
        
        //Calculation of the wait delay time according to the game speed configuration
        float delay = baseDelayBetweenLightUp / levelConfiguration.gameSpeed;
        
        for (int i = startIndex; i < playSequnce.Count; i++)
        {
            GameButton gameButton = playSequnce[i];
            
            yield return new WaitForSeconds(delay);

            gameButton.SetButtonSprite(true);
            gameButton.buttonSound.Play();

            yield return new WaitForSeconds(0.2f);
            gameButton.SetButtonSprite(false);
        }
        
        ChangeState(GameState.PlayerTurn);
    }
    
    private bool ChangeState(GameState newState)
    {
        bool didChange = false;
        switch (currentState)
        {
            case GameState.Idle:
                if (newState != GameState.SequencePlay) break;
                countdownCoroutine = StartCoroutine(CountdownRoutine());
                currentState = newState;
                didChange = true;
                break;
            
            case GameState.SequencePlay:
                if (newState != GameState.PlayerTurn && newState != GameState.End) break;
                currentState = newState;
                didChange = true;
                break;
            
            case GameState.PlayerTurn:
                if (newState != GameState.SequencePlay && newState != GameState.End) break;
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
            case GameState.Idle:
                break;
            case GameState.SequencePlay:
                PlaySequence();
                break;
            case GameState.PlayerTurn:
                playerCurPlaySequenceListIndex = 0;
                break;
            case GameState.End:
                if (didPlayerWin) {
                    SendPlayerScore?.Invoke(score);
                }
                else {
                    StopCoroutine(countdownCoroutine);
                    SendPlayerScore?.Invoke(-1);
                }
                break;
        }
    }

    public enum GameState 
    {
        Idle,
        SequencePlay,
        PlayerTurn,
        End
    }
}
