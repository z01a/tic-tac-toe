using System;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : Menu
{
    [Header("Game Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    public Button backButton;

    void Start()
    {
        Debug.Log("GameMenu Start");
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
