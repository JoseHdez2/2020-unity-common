using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRPGUnit : MonoBehaviour
{
    public string id;
    public string name;
    public string typeId;
    public int hp = 10;
    public int attack = 1;
    public int defense = 1;

    public State state = State.Idle;

    public enum State {
        Idle,
        Moving,
        Disabled
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
