using System;
using UnityEngine;

public abstract class Pausable : MonoBehaviour
{
    protected bool isPaused = false;

    public void Pause(bool pause)
    {
        if (isPaused == pause) return;
        isPaused = pause;
        OnPause(isPaused);
    }


    protected void Update()
    {
        if (isPaused)
        {
            return;
        } else {
            Update2();
        }
    }

    protected void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }
        else
        {
            FixedUpdate2();
        }
    }

    protected abstract void Update2();
    protected abstract void FixedUpdate2();
    protected abstract void OnPause(bool isPaused);
}
