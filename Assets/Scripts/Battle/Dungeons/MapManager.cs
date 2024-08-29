using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    public GameObject cellObject;  // ���� �ش��ϴ� ������Ʈ (�� ������Ʈ)
    public bool isBlocked;         // �� ���� ���� �ִ��� ����
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

    // Start is called before the first frame update
    void Start()
    {
        player_Pos = new Vector2Int(0, 0);
        SetRoom(player_Pos);
    }

    public void ShowMoveToRoomUI()
    {
        // �� �̵� UI ����
        // UI

        UpdateRoomUI();

    }

    private void UpdateRoomUI()
    {
        // ���� �濡 ���� ��ư Ȱ��ȭ �� Ȱ��ȭ ����
    }


    private void SetRoom(Vector2Int cur_Pos)
    {
        // ó�� ���� �� ó�� ���� ������ ������ ���� ��Ȱ��ȭ
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

        // �̴ϸ� ���� �ڵ� �־�ߵǳ�?
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
            case 0: // �Ʒ���
                MoveToRoom(new Vector2Int(0, -1));
                break;
            case 1: // ����
                MoveToRoom(new Vector2Int(0, 1));
                break;
            case 2: // ����
                MoveToRoom(new Vector2Int(-1, 0));
                break;
            case 3: // ������
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



