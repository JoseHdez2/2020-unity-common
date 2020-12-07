using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using ExtensionMethods;
using System.Linq;

[Serializable]
public class ProcFpsPrefab {
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 eulerAngles = new Vector3();
}

/// <summary>
/// Takes 3d "tilemaps" and instantiates them, translating each character to a prefab.
/// </summary>
public class ProcFpsConstructor : MonoBehaviour {
    public SerializableDictionaryBase<char, ProcFpsPrefab> prefabs;
}