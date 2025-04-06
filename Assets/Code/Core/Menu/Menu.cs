using UnityEngine;

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