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
    }


}



