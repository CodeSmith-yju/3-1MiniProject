using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public GameObject cellObject;  // 셀에 해당하는 오브젝트 (방 오브젝트)
    public bool isBlocked;         // 이 셀이 막혀 있는지 여부
}


[System.Serializable]
public class Row
{
    public List<Cell> cells; // 한 행의 셀(방) 리스트
}


public class MapManager : MonoBehaviour
{
    public List<Row> mapRows;
    private Vector2Int player_Pos;

    // Start is called before the first frame update
    void Start()
    {
        player_Pos = new Vector2Int(0, 0);
    }


}



