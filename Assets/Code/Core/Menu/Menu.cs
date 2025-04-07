using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    [Header("Menu Scene Reference")]
    [Tooltip("Reference to the scene this menu is associated with, leave empty if not used")]
    [SerializeField]
    public SceneReference SceneReference;

    void OnValidate()
    {
        SceneReference?.OnValidate();
    }

    protected void AddOnClickListener(Button button, Action callback)
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

    protected void AddOnValueChangedListener(Toggle toggle, Action<bool> callback)
    {
        if(toggle != null)
        {
            toggle.onValueChanged.AddListener((value) => callback(value));
        }
        else
        {
            Log.Warn($"Toggle {toggle.name} is not assigned in the inspector.");
        }
    }
    public virtual void Show()
    {
        Log.Info($"Showing menu: {gameObject.name}");
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        Log.Info($"Hiding menu: {gameObject.name}");
        gameObject.SetActive(false);
    }
}