using UnityEngine;

[System.Serializable]
public class SrpgSettings {
    [SerializeField] public bool showCancelButton = false;
    [SerializeField] public bool showStatusButton = false;
    [SerializeField] public bool friendlyFire = false;

    public SrpgSettings(){
        if (Application.platform == RuntimePlatform.Android){
            Debug.Log("Android platform detected. Forcing Android settings.");
            showCancelButton = true;
        }
    }
}