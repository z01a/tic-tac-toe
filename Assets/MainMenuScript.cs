using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [Tooltip("Button to start the game")]
    [SerializeField]
    public Button playButton;

    [Tooltip("Button to view game statistics")]
    [SerializeField]
    public Button statsButton;

    [Tooltip("Button to open settings")]
    [SerializeField]
    public Button settingsButton;

    [Tooltip("Button to exit the game")]
    [SerializeField]
    public Button exitButton;

    void Start()
    {
        if(playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        if(statsButton != null)
        {
            statsButton.onClick.AddListener(OnStatsButtonClicked);
        }
        if(settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }
        if(exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }
    private void OnPlayButtonClicked()
    {
        throw new NotImplementedException();
    }
    private void OnStatsButtonClicked()
    {
        throw new NotImplementedException();
    }
    private void OnSettingsButtonClicked()
    {
        throw new NotImplementedException();
    }
    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
