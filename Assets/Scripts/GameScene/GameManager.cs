using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//Documentation
// 1. Game Mangaer will define the levelData accroding to Difficulty level 
// 2. Game manager will keep scoring
// 3. Game Manger will keep the player Name;
// 4. Game manager will keep state machine to change states of game - pasue start stop win lose;
// 5. Game Manager will update score in win stare in table scroe
// 6. Game Manager will return to start screen for now
// 7. manage click button with boolean? or maybe disable
//Set interactable to false and set the alpha for the disabled color to 0, so it will be totally transparent and not visible, even when you hover with the mouse on it.
// 8. Just use list and compare by index

public class GameManager : MonoBehaviour
{
    public GameManagerState currentState { get; private set; } = GameManagerState.Idle;
    private List<Button> playSequnce = new List<Button>();
    
    private LevelData levelData = LevelData.Instance;

    private int scoring;
    [SerializeField] private TMP_Text scoreText;
    private float countdownTime;
    [SerializeField] private TMP_Text countdownText;
    private Coroutine countdownCoroutine;
    
    private int playerCurPlaySequenceListIndex;
    private float baseDelayBetweenLightUp = 0.5f;
    
    [SerializeField] private GameObject board;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Button [] gameButtons;
    
    private Color[] gameButtonsColors;
    [SerializeField] private AudioClip[] gameButtonsSounds;
    
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private Sprite topButtonSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        InitiateLevelDate();
        GenerateButtonColorsList();
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
    }

    private void Unsubscribe()
    {
        PreGameWindow.PlayerDidPressStart -= OnPlayerDidPressStart;
    }

    private void OnPlayerDidPressStart()
    {
        ChangeState(GameManagerState.SequencePlay);
    }

    private void InitiateLevelDate()
    {
        Dictionary<string, object> levelConfiguration = LevelConfigurationHolder.Configuration;
        int a = (int)levelConfiguration[GameConfigurationKeys.NumOfGameButtons];
        bool b = (bool)levelConfiguration[GameConfigurationKeys.RepeatMode];
        levelData.UpdateLevelData(
            (int)levelConfiguration[GameConfigurationKeys.NumOfGameButtons],
            (int)levelConfiguration[GameConfigurationKeys.PointsPerStep],
            (int)levelConfiguration[GameConfigurationKeys.GameTime],
            (bool)levelConfiguration[GameConfigurationKeys.RepeatMode],
            (float)levelConfiguration[GameConfigurationKeys.GameSpeed]);
    }
    
    private void GenerateButtonColorsList()
    {
        gameButtonsColors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan
        };
    }

    private void InitiateTime()
    {
        countdownTime = levelData.gameTime;
        countdownText.text = $"Time: {countdownTime}";
    }
    private void InitiateBoard()
    {
        gameButtons = new Button[levelData.numOfGameButtons];
        for (int i = 0; i < gameButtons.Length; i++)
        {
            Button newButton = Instantiate(buttonPrefab, board.transform);
            
            //decide if we shuold throw exception
            Image buttonImage = newButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = gameButtonsColors[i];
            }
            else
            {
                Debug.LogError("Image component not found on newButton");
            }

            AudioSource buttonSound = newButton.GetComponent<AudioSource>();
            if (buttonSound != null)
            {
                buttonSound.clip = gameButtonsSounds[i];
            }
            else
            {
                Debug.LogError("AudioSource component not found on newButton");
            }

            
            newButton.onClick.AddListener(() => OnGameButtonClick(newButton));
            gameButtons[i] = newButton;
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

        ChangeState(GameManagerState.Win);
    }
    
    private void OnGameButtonClick(Button gameButton)
    {
        if (currentState == GameManagerState.PlayerTurn)
        {
            StartCoroutine(PlayerButtonClickRoutine(gameButton));
        }
    }

    private IEnumerator PlayerButtonClickRoutine(Button gameButton)
    {
        yield return new WaitForSeconds(0.2f);
        
        gameButton.GetComponent<Image>().sprite = topButtonSprite;
        gameButton.GetComponent<AudioSource>().Play();
        scoring += levelData.pointsPerStep;
        scoreText.text = $"score: {scoring}";
        
        yield return new WaitForSeconds(0.2f);
        gameButton.GetComponent<Image>().sprite = buttonSprite;
        
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
        Button randomButton = gameButtons[randomButtonIndex];
        playSequnce.Add(randomButton);

        StartCoroutine(SequencePlayRoutine());
    }
    
    private IEnumerator SequencePlayRoutine()
    {
        int startIndex = 0;
        if (!levelData.repeatMode)
        {
            startIndex = playSequnce.Count - 1;
        }
        
        float delay = baseDelayBetweenLightUp / levelData.gameSpeed;
        
        for (int i = startIndex; i < playSequnce.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            
            Button gameButton = playSequnce[i];
            
            yield return new WaitForSeconds(0.2f);
        
            gameButton.GetComponent<Image>().sprite = topButtonSprite;
            gameButton.GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(0.2f);
            gameButton.GetComponent<Image>().sprite = buttonSprite;
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
                Debug.Log("You Win");
                break;
            case GameManagerState.Lose:
                StopCoroutine(countdownCoroutine);
                Debug.Log("You losee");
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
