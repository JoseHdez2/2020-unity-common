using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsProcBounds : MonoBehaviour
{
    public string uuid;
    public FpsProcBldg bldg;
    BoxCollider boxColl;
    public int floorNum;

    private void Awake() {
        boxColl = GetComponent<BoxCollider>();
    }

    private void Start() {
        uuid = ToStr();
    }

    public string ToStr() => $"{bldg.data.name} F{floorNum}";

    private void OnTriggerEnter(Collider other) {
        FindObjectOfType<FpsProcGameMgr>().EnterFloor(this);
    }

    private void OnTriggerExit(Collider other) {
        FindObjectOfType<FpsProcGameMgr>().ExitFloor();
    }
}
