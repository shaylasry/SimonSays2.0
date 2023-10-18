using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserNameWindow : MonoBehaviour
{
    [SerializeField] private Button okButton;
    [SerializeField] private TMP_InputField inputFiled;
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnOkButtonClick()
    {
        PlayerPrefs.SetString(PlayerPrefsKeys.UserName, inputFiled.text);
    }
}