using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using ExtensionMethods;
using System.Linq;

/// <summary>
/// A 3d prefab along with rotation angles.
/// </summary>
[Serializable]
public class ProcFpsPrefab {
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 eulerAngles = new Vector3();
}

/// <summary>
/// For taking 3d "tilemaps" and instantiating them, translating each character to a prefab.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "PrefabDict_", menuName = "ScriptableObjects/CharToPrefabDict", order = 1)]
public class CharToPrefabDict : ScriptableObject {
    public SerializableDictionaryBase<char, ProcFpsPrefab> prefabs;
}