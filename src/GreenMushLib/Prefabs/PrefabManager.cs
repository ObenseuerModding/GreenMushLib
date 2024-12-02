using System.Collections.Generic;
using UnityEngine;

namespace GreenMushLib.Prefabs;

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

    public static GameObject? GetCollectablePrefabByName(string name)
    {
        if (CollectablePrefabList.TryGetValue(name, out var thing))
        {
            return thing;
        }

        return null;
    }
}
