using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsProcBounds : MonoBehaviour
{
    public FpsProcBldg bldg;
    BoxCollider boxColl;
    public int floorNum;

    private void Awake() {
        boxColl = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        FindObjectOfType<FpsProcGameManager>().EnterFloor(bldg.data, floorNum);
    }

    private void OnTriggerExit(Collider other) {
        FindObjectOfType<FpsProcGameManager>().ExitFloor();
    }
}
