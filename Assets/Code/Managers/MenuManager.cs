using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Menu Manager")]
    [Tooltip("Reference to the scene this menu manager is associated with")]
    [SerializeField]
    private SceneReference _defaultSceneReference;
    [Header("Menus")]
    [Tooltip("List of menus to be managed")]
    [SerializeField]
    private List<GameObject> _menusPrefabs = new();

    private Dictionary<Type, Menu> _menus = new();

    private Stack<Menu> _menuStack = new();

    void OnValidate()
    {
        _defaultSceneReference?.OnValidate();
    }

    void Start()
    {
        InstantiateMenus();

        // Always navigate to the main menu on start
        // TODO: Move this to a state machine or similar
        NavigateTo<MainMenu>();
    }

    private void InstantiateMenus()
    {
        // TODO: Do not instantiate all menus at once, only the ones needed
        foreach (GameObject menuPrefab in _menusPrefabs)
        {   
            GameObject menuInstance = Instantiate(menuPrefab, transform);
            if(menuInstance != null)
            {
                Log.Info($"Instantiated menu: {menuInstance.name}");
                menuInstance.name = menuPrefab.name;

                Menu menu = menuInstance.GetComponent<Menu>();
                if(menu != null)
                {
                    _menus.Add(menu.GetType(), menu);
                }
            }
        }
    }

    public void NavigateTo<T>() where T : Menu
    {
        if (_menus.TryGetValue(typeof(T), out var menu))
        {
            if (_menuStack.TryPeek(out var currentMenu))
            {
                if (currentMenu != menu)
                {

                    currentMenu.Hide();
                    Log.Info($"Pushing menu: {menu.name} to stack");
                    _menuStack.Push(menu);
                }
            }
            else
            {
                Log.Info($"Pushing menu: {menu.name} to stack");
                _menuStack.Push(menu);
            }

            if(_menuStack.TryPeek(out var topMenu))
            {
                ChangeSceneIfNeeded();

                topMenu.Show();
            }
            else
            {
                Log.Info("No menu found in the stack.");
            }
        }
    }

    private void ChangeSceneIfNeeded()
    {
        if(_menuStack.TryPeek(out var topMenu))
        {
            if(topMenu.SceneReference.SceneName != string.Empty)
            {
                if(SceneManager.GetActiveScene().name != topMenu.SceneReference.SceneName)
                {
                    Log.Info($"Current scene: {SceneManager.GetActiveScene().name}");
                    Log.Info($"Changing scene to: {topMenu.SceneReference.SceneName}");
                    SceneManager.LoadScene(topMenu.SceneReference.SceneName, LoadSceneMode.Single);
                }
            }
            else if(SceneManager.GetActiveScene().name != _defaultSceneReference.SceneName)
            {
                Log.Info($"Current scene: {SceneManager.GetActiveScene().name}");
                Log.Info($"Changing scene to: {_defaultSceneReference.SceneName}");

                SceneManager.LoadScene(_defaultSceneReference.SceneName, LoadSceneMode.Single);
            }
            else
            {
                Log.Info($"Scene change is not needed!)");
            }
        }
    }

    public void NavigateBack()
    {
        if(_menuStack.Any())
        {
            var currentMenu = _menuStack.Pop();
            Log.Info($"Popping menu: {currentMenu.name} from stack");

            currentMenu.Hide();

            if(_menuStack.TryPeek(out var topMenu))
            {
                ChangeSceneIfNeeded();

                topMenu.Show();
            }
            else
            {
                Log.Info("No menu found in the stack.");
            }
        }
    }
}
