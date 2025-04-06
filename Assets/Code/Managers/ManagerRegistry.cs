using System.Collections.Generic;
using UnityEngine;

public class ManagerRegistry : Singleton<ManagerRegistry>
{
    [Header("Managers")]
    [Tooltip("Managers to be instantiated")]
    [SerializeField]
    private List<GameObject> _managersPrefabs = new List<GameObject>();

    void Start()
    {
        InstantiateManagers();
    }

    private void InstantiateManagers()
    {
        foreach (GameObject managerPrefab in _managersPrefabs)
        {
            GameObject managerInstance = Instantiate(managerPrefab, transform);
            if (managerInstance != null)
            {
                Log.Info($"Instantiated manager: {managerInstance.name}");
                managerInstance.name = managerPrefab.name;
            }
        }
    }
}
