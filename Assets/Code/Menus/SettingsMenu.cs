using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    [Header("Settings Menu Buttons")]
    [Tooltip("Button to go back")]
    [SerializeField]
    public Button backButton;

    void Start()
    {
        Debug.Log("SettingsMenu Start");
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
