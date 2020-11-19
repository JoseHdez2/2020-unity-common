using UnityEngine;
using System.Collections.Generic;

public abstract class StateMachine<T>  {

    private Stack<T> stateStack;

    public void HandleCancel(){
        T prevState = stateStack.Pop();
        GoToState(prevState);
        HandleCancelChild();
    }

    public abstract void HandleCancelChild(); // cancel sound, etc.

    public abstract void GoToState(T state);
}