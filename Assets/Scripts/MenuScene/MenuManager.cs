using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    //Use c# event so each game object can handle it's own functionality and we can still manage passing between Menu scene states
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
    
    //Using a state machine to handle the different states in the Menu Scene
    public enum MenuManagerState 
    {
        UserNameInsertion,
        LevelSelection,
        StartGame
    }
}
