using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    [Header("Settings Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    private Button backButton;

    [Header("Settings Menu Toggle")]
    [Tooltip("Turn off music")]
    [SerializeField]
    private Toggle toggleMusic;

    [Tooltip("Turn off SFX")]
    [SerializeField]
    private Toggle toggleSfx;

    void Start()
    {
        Debug.Log("SettingsMenu Start");
        AddOnClickListener(backButton, OnBackButtonClicked);
        
        AddOnValueChangedListener(toggleMusic, OnToggleMusicChanged);
        AddOnValueChangedListener(toggleSfx, OnToggleSFXChanged);
    }

    private void OnToggleSFXChanged(bool value)
    {
        SoundManager.Instance.ToggleSFX(!value);
    }

    private void OnToggleMusicChanged(bool value)
    {
        SoundManager.Instance.ToggleMusic(!value);
    }

    private void OnBackButtonClicked()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");

        MenuManager.Instance.NavigateBack();
    }
}
