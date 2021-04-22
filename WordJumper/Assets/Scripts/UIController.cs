using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AnotherFileBrowser.Windows;
using System.IO;

public class UIController : MonoBehaviour
{
    public Canvas _MainMenu, _Settings, _HowToPlay, _Credits, _FormatError;
    public Button _PlayButton;

    public enum STATE
    {
        MAINMENU,
        SETTINGS,
        HOWTOPLAY,
        CREDITS,
        FORMATERROR
    };


    public void Start()
    {
        _PlayButton.enabled = false;
        _PlayButton.image.color = _PlayButton.colors.disabledColor;
        SetState(STATE.MAINMENU);        
    }   


    public void SetState(STATE state) 
    {
        // state machine for ui elements
        switch (state) 
        {
            case STATE.MAINMENU:
                _MainMenu.enabled = true;
                _Settings.enabled = false;
                _HowToPlay.enabled = false;
                _Credits.enabled = false;
                _FormatError.enabled = false;
                break;
            case STATE.SETTINGS:
                _MainMenu.enabled = false;
                _Settings.enabled = true;
                break;
            case STATE.HOWTOPLAY:
                _MainMenu.enabled = false;
                _HowToPlay.enabled = true;
                break;
            case STATE.CREDITS:
                _MainMenu.enabled = false;
                _Credits.enabled = true;
                break;
            case STATE.FORMATERROR:
                _MainMenu.enabled = false;
                _FormatError.enabled = true;
                break;
            default:
                break;
        }
    }

    public void SetLanguage() { }

    public void OnSettingsClicked()
    {
        SetState(STATE.SETTINGS);
    }

    public void OnCreditsClicked()
    {
        SetState(STATE.CREDITS);
    }

    public void OnHowToClicked()
    {
        SetState(STATE.HOWTOPLAY);
    }

    public void OnBackClicked() 
    {
        SetState(STATE.MAINMENU);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void SetFilePath(string path) 
    {
        // load data and enable play button
        QAContainer qac = GameObject.FindGameObjectWithTag("QAContainer").GetComponent<QAContainer>();
        if (!qac.LoadValues(path)) 
        {
            SetState(STATE.FORMATERROR);
            return; 
        }     

        string[] pathsplit = path.Split('\\');
        string[] pathsplit2 = pathsplit[pathsplit.Length - 1].Split('.');

        var txtcomp =_PlayButton.GetComponentInChildren<Text>();
        txtcomp.text = txtcomp.text + " '" + pathsplit2[0] + "'";

        _PlayButton.enabled = true;
        _PlayButton.image.color = _PlayButton.colors.normalColor;
    }  

    public void FileDialog()
    {
        // file open dialog
        BrowserProperties properties = new BrowserProperties();
        properties.filter = "Text File (*.txt) | *.txt";
        properties.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(properties, path =>
        {
            SetFilePath(path);
        });
    }

}
