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
        SetRoom(player_Pos);
    }

    public void ShowMoveToRoomUI()
    {
        // 방 이동 UI 띄우기
        // UI

        UpdateRoomUI();

    }

    private void UpdateRoomUI()
    {
        // 현재 방에 따라 버튼 활성화 비 활성화 할지
    }


    private void SetRoom(Vector2Int cur_Pos)
    {
        // 처음 입장 시 처음 방을 제외한 나머지 방은 비활성화
        foreach (Row row in mapRows) 
        {
            foreach(Cell cell in row.cells)
            {
                if (mapRows[cur_Pos.x].cells[cur_Pos.y] != cell)
                {
                    cell.cellObject.SetActive(false);
                }
            }
        }

        // 미니맵 관련 코드 넣어야되나?
    }


    private Cell GetRoom(Vector2Int room)
    {
        for (int x = 0; x < mapRows.Count; x++) 
        {
            for (int y = 0; y < mapRows[x].cells.Count; y++) 
            {
                if (mapRows[x].cells[y] == mapRows[room.x].cells[room.y])
                {
                    return mapRows[x].cells[y];
                }
            }
        }

        return null;
    }


    public void MoveToRoomButton(int way)
    {
        switch (way) 
        {
            case 0: // 아래로
                MoveToRoom(new Vector2Int(0, -1));
                break;
            case 1: // 위로
                MoveToRoom(new Vector2Int(0, 1));
                break;
            case 2: // 왼쪽
                MoveToRoom(new Vector2Int(-1, 0));
                break;
            case 3: // 오른쪽
                MoveToRoom(new Vector2Int(1, 0));
                break;
        }
    }


    private void MoveToRoom(Vector2Int targetRoom)
    {
        Vector2Int relativePos = player_Pos + targetRoom;
        StartCoroutine(MoveToCamera(GetRoom(relativePos).cellObject.transform));

        player_Pos = relativePos;
    }


    private IEnumerator MoveToCamera(Transform targetRoom)
    {

          

        yield return null;
    }


}



