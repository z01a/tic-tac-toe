using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : Menu
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
        AddButtonOnClickListener(playButton, OnPlayButtonClicked);
        AddButtonOnClickListener(statsButton, OnStatsButtonClicked);
        AddButtonOnClickListener(settingsButton, OnSettingsButtonClicked);
        AddButtonOnClickListener(exitButton, OnExitButtonClicked);
    }

    private void AddButtonOnClickListener(Button button, Action callback)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => callback());
        }
        else
        {
            Log.Warn($"Button {button.name} is not assigned in the inspector.");
        }
    }

    private void OnPlayButtonClicked()
    {
        MenuManager.Instance.NavigateTo<GameMenu>();
    }
    private void OnStatsButtonClicked()
    {

        MenuManager.Instance.NavigateTo<StatsMenu>();
    }
    private void OnSettingsButtonClicked()
    {
        MenuManager.Instance.NavigateTo<SettingsMenu>();
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
