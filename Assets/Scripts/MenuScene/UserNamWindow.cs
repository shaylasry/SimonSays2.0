using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserNameWindow : MonoBehaviour
{
    public static Action PlayerDidEnterUserName;

    private UserData userData = UserData.Instance;
    [SerializeField] private Button okButton;
    [SerializeField] private TMP_InputField inputFiled;
    
    private void Start()
    {
        okButton.onClick.AddListener(OnOkButtonClick);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnOkButtonClick()
    {
        userData.userName = inputFiled.text;
        PlayerDidEnterUserName?.Invoke();
    }
}
