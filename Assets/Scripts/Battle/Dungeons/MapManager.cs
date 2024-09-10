using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cell
{
    [Header("Room")]
    public GameObject cellObject;  // 셀에 해당하는 오브젝트 (방 오브젝트)
    public bool isClear;           // 방 클리어 여부
    public bool isBlocked;         // 이 셀이 막혀 있는지 여부
    public bool isBoss;            // 방이 보스방인지 여부

    [Header("Minimap")]
    public GameObject minimap_Obj;
    public bool isVisit;
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
    private Vector2Int select_Direction;
    public Transform cur_Room;
    public Camera map_Camera;
    public Camera map_Camera_Big;
    public bool isMoveDone = false;
    public List<GameObject> map_Icon; // 0 : 위, 1 : 아래, 2 : 왼쪽, 3 : 오른쪽


    Vector2Int[] directions = new Vector2Int[] // 방향 배열
    {
            new Vector2Int(0, -1),  // 위
            new Vector2Int(0, 1), // 아래
            new Vector2Int(-1, 0), // 왼쪽
            new Vector2Int(1, 0)   // 오른쪽
    };



    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        player_Pos = new Vector2Int(0, 0);
        SetRoom(player_Pos);
        MiniMapUpdate(player_Pos);
    }

    public void ShowMoveToRoomUI() // 포탈에 쓸 메서드
    {
        // 방 이동 UI 띄우기
        BattleManager.Instance.ui.room_UI.SetActive(true);
        // UI 업데이트
        UpdateRoomUI();
    }

    public void HideRoomUI()
    {
        BattleManager.Instance.ui.room_UI.SetActive(false);
    }

    private void MiniMapUpdate(Vector2Int cur_Room) // 이동 할 때 마다 인접한 방 표시 미니맵 업데이트
    {
        mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.SetActive(true);
        mapRows[cur_Room.y].cells[cur_Room.x].isVisit = true; // 현재방 방문
        mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.white; // 현재방 색깔은 하얀색으로 변경

        for (int i = 0; i < directions.Length; i++) 
        {
            Vector2Int direction = directions[i];
            Vector2Int newPos = cur_Room + direction;

            
            if (IsValid(newPos) && !IsBlock(newPos))
            {
                mapRows[newPos.y].cells[newPos.x].minimap_Obj.SetActive(true); // 상하좌우로 인접한 방
                mapRows[newPos.y].cells[newPos.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.gray; // 상하좌우로 인접한 방은 회색으로 변경
            }
        }
    }

    public void OpenMap(bool isOpen)
    {

        map_Camera_Big.gameObject.SetActive(isOpen);
        BattleManager.Instance.ui.mini_Map_Big.SetActive(isOpen);
        BattleManager.Instance.ui.mini_Map.SetActive(!isOpen);
        map_Camera.gameObject.SetActive(!isOpen);

        OpenMapUpdate(isOpen);
    }

    private void OpenMapUpdate(bool updateMap)
    {
        if (updateMap)
        {
            foreach (Row row in mapRows)
            {
                foreach (Cell cell in row.cells)
                {
                    if (cell.isVisit)
                    {
                        cell.minimap_Obj.SetActive(true);

                        if (cell.cellObject == cur_Room.gameObject)
                        {
                            cell.minimap_Obj.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        else
                        {
                            cell.minimap_Obj.GetComponent<SpriteRenderer>().color = Color.gray;
                        }
                    }
                    else
                    {
                        cell.minimap_Obj.SetActive(false);
                    }
                }
            }
        }
        else
        {

            // 끄면 미니맵 전부 비활성화 후 다시 계산해서 미니맵 활성화
            foreach (Row row in mapRows)
            {
                foreach (Cell cell in row.cells)
                {
                    cell.minimap_Obj.SetActive(false);
                }
            }

            MiniMapUpdate(player_Pos);
        }
        
    }

    // 큰 미니맵은 어떤식으로 하지?


    private void UpdateRoomUI() // 현재 방에 따라 버튼 활성화 비 활성화 할지
    {
        // 네 방향을 배열로 정의
        // 각 방향에 따라 map_Icon을 설정
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int direction = directions[i];
            Vector2Int newPos = player_Pos + direction;

            if (IsValid(newPos) && !IsBlock(newPos))
            {
                map_Icon[i].SetActive(true);
            }
            else
            {
                map_Icon[i].SetActive(false); // 이동 불가능한 경우 비활성화
            }
        }
    }

    private bool IsValid(Vector2Int pos) // 전체 방 크기 확인
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < mapRows[0].cells.Count && pos.y < mapRows.Count;
    }

    private bool IsBlock(Vector2Int pos) // 방이 막힌 방인지 확인
    {
        Cell cell = mapRows[pos.y].cells[pos.x];

        return cell.isBlocked;
    }


    private void SetRoom(Vector2Int cur_Pos) // 처음 입장 시 처음 방을 제외한 나머지 방은 비활성화
    {
        
        foreach (Row row in mapRows) 
        {
            foreach(Cell cell in row.cells)
            {
                if (mapRows[cur_Pos.y].cells[cur_Pos.x] != cell)
                {
                    cell.cellObject.SetActive(false);
                    cell.minimap_Obj.SetActive(false);
                }
                else
                {
                    cell.cellObject.SetActive(true);
                    cur_Room = cell.cellObject.transform;
                }
            }
        }
    }


    private Cell GetRoom(Vector2Int room) // Vector2Int 좌표 값에 해당하는 방 구하기
    {
        for (int y = 0; y < mapRows.Count; y++) 
        {
            for (int x = 0; x < mapRows[y].cells.Count; x++) 
            {
                if (mapRows[y].cells[x] == mapRows[room.y].cells[room.x])
                {
                    return mapRows[y].cells[x];
                }
            }
        }

        return null;
    }

    public Cell FindRoom(GameObject room)
    {
        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
            {
                if (cell.cellObject == room)
                {
                    return cell;
                }
            }
        }

        return null;
    }


    public void MoveToRoomButton(int way)
    {
        switch (way) 
        {
            case 0: // 위로
                select_Direction = new Vector2Int(0, -1);
                break;
            case 1: // 아래로
                select_Direction = new Vector2Int(0, 1);
                break;
            case 2: // 왼쪽
                select_Direction = new Vector2Int(-1, 0);
                break;
            case 3: // 오른쪽
                select_Direction = new Vector2Int(1, 0);
                break;
        }

        BattleManager.Instance.ui.room_UI.SetActive(false);
        BattleManager.Instance.ui.OpenPopup(BattleManager.Instance.ui.next_Room_Popup);
    }

    public void ConfirmRoom()
    {
        MoveToRoom(select_Direction);
    }


    private void MoveToRoom(Vector2Int targetRoom)
    {
        isMoveDone = false;

        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
            {
                if (cell.cellObject == cur_Room.gameObject)
                {
                    if (cell.isClear == false)
                    {
                        cell.isClear = true;
                    }
                }
            }
        }

        Vector2Int relativePos = player_Pos + targetRoom;
        player_Pos = relativePos;
        StartCoroutine(MoveToCamera(GetRoom(player_Pos).cellObject.transform));
        
    }


    private IEnumerator MoveToCamera(Transform targetRoom)
    {
        Vector3 cam_Target_Pos = new Vector3(targetRoom.position.x, targetRoom.position.y, Camera.main.transform.position.z);
        Vector3 targetMap = new Vector3(GetRoom(player_Pos).minimap_Obj.transform.position.x, GetRoom(player_Pos).minimap_Obj.transform.position.y, map_Camera.transform.position.z);

        cur_Room = targetRoom;

        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
            {
                if (targetRoom.gameObject == cell.cellObject)
                {
                    cell.cellObject.SetActive(true);
                    cell.minimap_Obj.SetActive(true);
                }
                else
                {
                    cell.cellObject.SetActive(false);
                    cell.minimap_Obj.SetActive(false);
                }
            }
        }

        

        while (Vector3.Distance(Camera.main.transform.position, cam_Target_Pos) > 0.1f && Vector3.Distance(map_Camera.transform.position, targetMap) > 0.1f)
        {
            if (Vector3.Distance(Camera.main.transform.position, cam_Target_Pos) > 0.1f)
            {
                Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, cam_Target_Pos, ref velocity, 0.2f);
            }

            if (Vector3.Distance(map_Camera.transform.position, targetMap) > 0.1f)
            {
                //map_Camera.transform.position = Vector3.SmoothDamp(map_Camera.transform.position, targetMap, ref velocity, 1f);
                map_Camera.transform.position = Vector3.Lerp(map_Camera.transform.position, targetMap, 1.5f * Time.deltaTime);
            }
            yield return null; // 다음 프레임까지 대기
        }

        // 나중에 미니맵 관련 추가

        Camera.main.transform.position = cam_Target_Pos;
        map_Camera.transform.position = targetMap;

        MiniMapUpdate(player_Pos);

        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
            {
                if (targetRoom.gameObject == cell.cellObject)
                {
                    foreach (Transform child in cell.cellObject.transform)
                    {
                        if (child.CompareTag("Enemy"))
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        isMoveDone = true;

        if (BattleManager.Instance.dialogue != null)
        {
            if (BattleManager.Instance.dialogue.isTutorial)
            {
                if (FindRoom(cur_Room.gameObject).isClear == false)
                {
                    BattleManager.Instance.dialogue.ONOFF(true);
                    BattleManager.Instance.dialogue.NextDialogue();
                }
            }
        }
        

        if (mapRows[player_Pos.y].cells[player_Pos.x].isClear)
        {
            BattleManager.Instance._curphase = BattleManager.BattlePhase.Rest;
        }
        else
        {
            BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start);
        }

        yield break;

    }


}



