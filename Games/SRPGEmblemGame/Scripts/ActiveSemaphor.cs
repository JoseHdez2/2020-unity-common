using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Keep a list of GameObjects where only one of them may be active at the same time.
public class ActiveSemaphor : MonoBehaviour {
    
    public List<GameObject> objects;

    public void SetActive(GameObject objToActivate){
        objects.ForEach(obj => obj.SetActive(false));
        objToActivate.SetActive(true);
    }

    public void SetActive<T>(){
        objects.ForEach(obj => obj.SetActive(false));
        var objToActivate = objects.First(obj => obj.GetComponent<T>() != null);
        objToActivate.SetActive(true);
    }

}