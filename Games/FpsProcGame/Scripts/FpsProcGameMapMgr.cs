using UnityEngine;
using TMPro;

public class FpsProcGameMapMgr : MonoBehaviour {
    [SerializeField] TMP_Text textAreaName, textAreaMap, textAreaMap2;

    public void EnterFloor(FpsProcBounds floor){
        textAreaName.text = floor.ToStr();
        textAreaMap.text = floor.bldg.data.TilemapToStr(floor.floorNum);
        textAreaMap2.text = floor.bldg.data.TilemapToStr2(floor.floorNum);
    }
    
    public void ExitFloor(){
        textAreaName.text = "";
        textAreaMap.text = "";
        textAreaMap2.text = "";
    }
}