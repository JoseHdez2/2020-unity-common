using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcFpsGeneratorBuilding : MonoBehaviour {
    [SerializeField] GameObject pfRoomSmall;
    [SerializeField] GameObject pfStairs;
    [SerializeField] Vector3Int gridSize;
    [SerializeField] Vector3 cellScale = Vector3.one;

    public void CreateBuilding(Vector3 origin, Vector3Int gridSize, Vector3 cellScale, int floors = 1){
        for(int i = 0; i < floors; i++){
            float heightOffset = i * cellScale.y;
            CreateFloor(new Vector3(origin.x, origin.y + heightOffset, origin.z), gridSize, cellScale);
        }
    }

    public void CreateFloor(Vector3 origin, Vector3Int gridSize, Vector3 cellScale){
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

    public List<List<string>> GenerateBuilding(Vector3Int gridSize){

        List<List<string>> grid = CreateCube(gridSize, ' ');
        return grid;

        // var watch = System.Diagnostics.Stopwatch.StartNew();
        // Debug.Log("${watch.ElapsedTicks}");
                
        // for(int x = 0; x < gridSize.x; x++){
        //     for(int y = 0; y < gridSize.y; y++){
        //         for(int z = 0; z < gridSize.z; z++){

        //         }
        //     }
        // }
    }

    private List<List<string>> CreateCube(Vector3Int gridSize, char c)
    {
        return Enumerable.Range(1, gridSize.z).Select(z => 
            Enumerable.Range(1, gridSize.y).Select(y => new string(c, gridSize.x)).ToList()
        ).ToList();
    }

    public char RandomTile(){
        float f = Random.Range(1,101);
        if(f < 80){
            return ' ';
        } else return '/';
    }
}