using UnityEngine;
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

    private void OnPlayButtonClicked()
    {
        DialogBox.Instance.Show("Do you want to play first as X?", 
            () => {
                GameManager.Instance.StartingPlayer = Player.X;
                MenuManager.Instance.NavigateTo<GameMenu>();
            }, 
            () => {
                GameManager.Instance.StartingPlayer = Player.O;
                MenuManager.Instance.NavigateTo<GameMenu>();
            });
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
        DialogBox.Instance.Show("Are you sure you want to exit?", 
            () => {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
            }, 
            () => {});
    }
}
