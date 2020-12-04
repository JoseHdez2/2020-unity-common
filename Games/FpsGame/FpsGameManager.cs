using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsGameManager : MonoBehaviour
{
    [SerializeField] GameObject pfRoomSmall;
    [SerializeField] GameObject pfStairs;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;

    void Start()
    {
        CreateBuilding(new Vector3(), gridSize, cellScale, 3);
    }

    private void CreateBuilding(Vector3 origin, Vector3Int gridSize, Vector3 cellScale, int floors = 1){
        for(int i = 0; i < floors; i++){
            float heightOffset = i * cellScale.y;
            CreateFloor(new Vector3(origin.x, origin.y + heightOffset, origin.z), gridSize, cellScale);
        }
    }



    private void CreateFloor(Vector3 origin, Vector3Int gridSize, Vector3 cellScale){
        for(int z = 0; z < gridSize.y; z++){
            for(int x = 0; x < gridSize.x; x++){
                float f = Random.Range(1,101);
                GameObject prefab;
                if(f < 50){
                    prefab = pfRoomSmall;
                } else {
                    prefab = pfStairs;
                }
                Instantiate(prefab, new Vector3(origin.x + x * cellScale.x, origin.y, origin.z + z * cellScale.z), Quaternion.identity);
            }
        }
    }

}
