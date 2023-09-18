using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//menu manager documentation:
// 1. load configuration from file
// 2. get user name
// 3. create 2 components -> input and chose screen
// 4. use state machine to define which part are we in, check if we shuold move from the choose screen or the game manger
// 5. present all the different choices for level difficulty
// 6. when player click the difficulty we will upadted Level data scriptabel object
//****Level Data and player data are scripatble objects so we cna use them for the whole game
//    and go so game scene -> we can use enum to decide which window to show

public class MenuManager : MonoBehaviour
{
    [SerializeField] private UserNameWindow userNameWindow;
    [SerializeField] private LevelSelectionMenu levelSelectionMenu;
    public MenuManagerState currentState { get; private set; } = MenuManagerState.UserNameInsertion;

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
        UserNameWindow.PlayerDidEnterUserName += OnPlayerDidEnterUserName;
        LevelSelectionMenu.PlayerDidPickLevel += OnPlayerDidPickLevel;
    }

    private void Unsubscribe()
    {
        UserNameWindow.PlayerDidEnterUserName -= OnPlayerDidEnterUserName;        
        LevelSelectionMenu.PlayerDidPickLevel -= OnPlayerDidPickLevel;
    }

    public void OnPlayerDidEnterUserName()
    {
        ChangeState(MenuManagerState.LevelSelection);
    }

    public void OnPlayerDidPickLevel()
    {
        ChangeState(MenuManagerState.StartGame);
    }
    
    private bool ChangeState(MenuManagerState newState)
    {
        bool didChange = false;
        switch (currentState)
        {
            case MenuManagerState.UserNameInsertion:
                userNameWindow.Hide();
                if (newState != MenuManagerState.LevelSelection) break;
                currentState = newState;
                didChange = true;
                break;
            
            case MenuManagerState.LevelSelection:
                if (newState != MenuManagerState.StartGame) break;
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
            case MenuManagerState.UserNameInsertion:
                userNameWindow.Show();
                break;
            case MenuManagerState.LevelSelection:
                levelSelectionMenu.Show();
                break;
            case MenuManagerState.StartGame:
                SceneManager.LoadScene("Game");
                break;
        }
    }
    
    public enum MenuManagerState 
    {
        UserNameInsertion,
        LevelSelection,
        StartGame
    }
}
