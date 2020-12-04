using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsGameManager : MonoBehaviour
{
    [SerializeField] GameObject pfRoomSmall;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] float cellScale = 1f;

    void Start()
    {
        for(int y = 0; y < gridSize.y; y++){
            for(int x = 0; x < gridSize.x; x++){
                Instantiate(pfRoomSmall, new Vector3(x * cellScale, 0, y * cellScale), Quaternion.identity);
            }
        }
    }

}
