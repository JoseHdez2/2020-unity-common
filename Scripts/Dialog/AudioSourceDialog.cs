using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EDialogSound { Char, FullStop, Next, WindowShow, WindowHide };

[RequireComponent(typeof(AudioSource))]
public class AudioSourceDialog : AudioSourceMultiBase<EDialogSound> { }