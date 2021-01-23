
using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[Serializable]
public class SpriteAnimation {
    [SerializeField] public List<Sprite> sprites;
}

[CreateAssetMenu(fileName = "SprAnim_", menuName = "ScriptableObjects/SpriteAnimByDir", order = 1)]
public class SpriteAnimsTopdown : ScriptableObject {
    public SerializableDictionaryBase<EDirection, SpriteAnimation> spritesDict;
    public EDirection flipDir = EDirection.LEFT;
    public float frameDuration = 0.5f;
}