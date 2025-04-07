using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : Menu
{
    [Header("Stats Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    private Button backButton;

    void Start()
    {
        AddOnClickListener(backButton, OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        // TODO: This should be moves somewhere else
        SoundManager.Instance.PlaySFX("click2");

        MenuManager.Instance.NavigateBack();
    }
}
