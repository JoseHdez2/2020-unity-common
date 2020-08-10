using UnityEngine;
using System.Collections;

public class InputUtils
{
    public static bool MovedJoystick()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
