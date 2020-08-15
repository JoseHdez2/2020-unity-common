using ExtensionMethods;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputMasterSingleton
{
    public static InputMaster controls;
    
    public static InputMaster Get()
    {
        if (controls == null)
        {
            controls = new InputMaster();
        }
        Debug.Log(controls);
        return controls;
    }
}
