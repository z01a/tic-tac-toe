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
        AddOnClickListener(playButton, OnPlayButtonClicked);
        AddOnClickListener(statsButton, OnStatsButtonClicked);
        AddOnClickListener(settingsButton, OnSettingsButtonClicked);
        AddOnClickListener(exitButton, OnExitButtonClicked);

        SoundManager.Instance.PlayMusic("music");
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
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");

        MenuManager.Instance.NavigateTo<StatsMenu>();
    }
    private void OnSettingsButtonClicked()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");

        MenuManager.Instance.NavigateTo<SettingsMenu>();
    }
    private void OnExitButtonClicked()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");

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
