using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FpsProcDistrict : MonoBehaviour {
    
    [SerializeField] List<FpsProcBldg> buildings;
    [SerializeField] private Vector3Int gridSize;
    [SerializeField] private int npcAmount;

    public void Start(){
        GenerateAndInstantiateBuildings();
    }

    public void GenerateAndInstantiateBuildings(){
        buildings = GetComponentsInChildren<FpsProcBldg>().ToList();
        foreach(FpsProcBldg building in buildings){
            GenerateAndInstantiate(building);
        }
    }

    private void GenerateAndInstantiate(FpsProcBldg building){
        building.Generate();
        Debug.Log($"something: {building.data.TilemapToStr()}");
        building.Instantiate(FindObjectOfType<FpsProcGameManager>().pfNpc);
    }

}