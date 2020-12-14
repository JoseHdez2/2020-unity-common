using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class FpsProcBounds : MonoBehaviour
{
    public string uuid;
    public FpsProcBldg bldg;
    BoxCollider boxColl;
    public int floorNum;
    public List<FpsProcNpc> npcs = new List<FpsProcNpc>();

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

    public void SetNpcToBounds(FpsProcNpc npc){
        npc.transform.position = RandomNpcSpawnPos();
        npcs.Add(npc);
    }

    public Vector3 RandomNpcSpawnPos(){
        Bounds floorBounds = bldg.data.GetFloorBounds(floorNum);
        float floorY = (floorBounds.min.y + floorBounds.max.y) / 2;
        return floorBounds.RandomPos().WithY(floorY - 0.25f);
    }      
}
