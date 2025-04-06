using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : Menu
{
    [Header("Stats Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    public Button backButton;

    void Start()
    {
        Debug.Log("StatsMenu Start");
        if(backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }
    }

    private void OnBackButtonClicked()
    {
        MenuManager.Instance.NavigateBack();
    }
}
