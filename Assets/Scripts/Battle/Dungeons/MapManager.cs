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
    public GameObject cellObject;  // ���� �ش��ϴ� ������Ʈ (�� ������Ʈ)
    public bool isClear;           // �� Ŭ���� ����
    public bool isBlocked;         // �� ���� ���� �ִ��� ����
    public bool isBoss;            // ���� ���������� ����

    [Header("Minimap")]
    public GameObject minimap_Obj;
    public bool isVisit;
}


[System.Serializable]
public class Row
{
    public List<Cell> cells; // �� ���� ��(��) ����Ʈ
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
    public List<GameObject> map_Icon; // 0 : ��, 1 : �Ʒ�, 2 : ����, 3 : ������


    Vector2Int[] directions = new Vector2Int[] // ���� �迭
    {
            new Vector2Int(0, -1),  // ��
            new Vector2Int(0, 1), // �Ʒ�
            new Vector2Int(-1, 0), // ����
            new Vector2Int(1, 0)   // ������
    };



    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        player_Pos = new Vector2Int(0, 0);
        SetRoom(player_Pos);
        MiniMapUpdate(player_Pos);
    }

    public void ShowMoveToRoomUI() // ��Ż�� �� �޼���
    {
        // �� �̵� UI ����
        BattleManager.Instance.ui.room_UI.SetActive(true);
        // UI ������Ʈ
        UpdateRoomUI();
    }

    public void HideRoomUI()
    {
        BattleManager.Instance.ui.room_UI.SetActive(false);
    }

    private void MiniMapUpdate(Vector2Int cur_Room) // �̵� �� �� ���� ������ �� ǥ�� �̴ϸ� ������Ʈ
    {
        mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.SetActive(true);
        mapRows[cur_Room.y].cells[cur_Room.x].isVisit = true; // ����� �湮
        mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.white; // ����� ������ �Ͼ������ ����

        for (int i = 0; i < directions.Length; i++) 
        {
            Vector2Int direction = directions[i];
            Vector2Int newPos = cur_Room + direction;

            
            if (IsValid(newPos) && !IsBlock(newPos))
            {
                mapRows[newPos.y].cells[newPos.x].minimap_Obj.SetActive(true); // �����¿�� ������ ��
                mapRows[newPos.y].cells[newPos.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.gray; // �����¿�� ������ ���� ȸ������ ����
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

            // ���� �̴ϸ� ���� ��Ȱ��ȭ �� �ٽ� ����ؼ� �̴ϸ� Ȱ��ȭ
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

    // ū �̴ϸ��� ������� ����?


    private void UpdateRoomUI() // ���� �濡 ���� ��ư Ȱ��ȭ �� Ȱ��ȭ ����
    {
        // �� ������ �迭�� ����
        // �� ���⿡ ���� map_Icon�� ����
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
                map_Icon[i].SetActive(false); // �̵� �Ұ����� ��� ��Ȱ��ȭ
            }
        }
    }

    private bool IsValid(Vector2Int pos) // ��ü �� ũ�� Ȯ��
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < mapRows[0].cells.Count && pos.y < mapRows.Count;
    }

    private bool IsBlock(Vector2Int pos) // ���� ���� ������ Ȯ��
    {
        Cell cell = mapRows[pos.y].cells[pos.x];

        return cell.isBlocked;
    }


    private void SetRoom(Vector2Int cur_Pos) // ó�� ���� �� ó�� ���� ������ ������ ���� ��Ȱ��ȭ
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


    private Cell GetRoom(Vector2Int room) // Vector2Int ��ǥ ���� �ش��ϴ� �� ���ϱ�
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
            case 0: // ����
                select_Direction = new Vector2Int(0, -1);
                break;
            case 1: // �Ʒ���
                select_Direction = new Vector2Int(0, 1);
                break;
            case 2: // ����
                select_Direction = new Vector2Int(-1, 0);
                break;
            case 3: // ������
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
            yield return null; // ���� �����ӱ��� ���
        }

        // ���߿� �̴ϸ� ���� �߰�

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



