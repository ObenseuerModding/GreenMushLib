using System.Collections.Generic;
using UnityEngine;

namespace GreenMushLib.Prefabs;

/// <summary>
/// A static class that contains lists of game prefabs sorted by name
/// </summary>
public static class PrefabsManager
{
    private static readonly Dictionary<string, GameObject> CollectablePrefabList = [];

    static PrefabsManager()
    {
        GameObject[] items = Resources.LoadAll<GameObject>("Prefabs/Items");
        Plugin.Log.LogInfo($"Loaded {items.Length} total collectable prefabs");

        foreach (var item in items)
        {
            CollectablePrefabList[item.name] = item;
        }
    }

    /// <summary>
    /// Retrieves a collectable object from the list by name
    /// </summary>
    /// <param name="name">This is the game object's name</param>
    /// <example>
    /// <code>
    /// GetCollectablePrefabByName("Can opener");
    /// </code>
    /// </example>
    /// <returns>A GameObject or null if none is found</returns>
    public static GameObject? GetCollectablePrefabByName(string name)
    {
        if (CollectablePrefabList.TryGetValue(name, out var thing))
        {
            return thing;
        }

        return null;
    }
}
