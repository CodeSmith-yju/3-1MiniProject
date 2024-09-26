using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Cell2
{
    public GameObject cellObject;  // �� ������Ʈ (������)
    public bool isClear;           // �� Ŭ���� ����
    public bool isBlocked;         // ���� ���� �ִ��� ���� (���� ���� ��� true)
    public bool isBoss;            // ������ ����
}

[System.Serializable]
public class Row2
{
    public List<Cell2> cells = new List<Cell2>();  // �� ���� �� ����Ʈ
}



public class TestMap : MonoBehaviour
{
    public List<Row2> mapRows = new List<Row2>();  // 4x4 �׸����� �� �迭
    public GameObject startPrefab;
    public GameObject[] battleRoomPrefabs;       // ������ �����յ�
    public GameObject chestRoomPrefab;           // ���ڹ� ������
    public GameObject restRoomPrefab;            // �޽Ĺ� ������
    public GameObject blockedRoomPrefab;         // ���� �� ������
    public GameObject bossRoomPrefab;            // ������ ������
    public Vector2Int player_Pos; // �÷��̾� ���� ��ġ
    public int gridSize = 4;                     // 4x4 �׸��� ũ��
    public int roomSpacing = 25;                 // �� ����

    // �ּ� �� ����
    public int maxBlockedRooms = 4;              // �ִ� ���� �� ����
    public int minChestRooms = 1;                // �ִ� ���� �� ����
    public int minRestRooms = 1;
    public int minBattleRooms = 3;

    private List<Vector2Int> availableBossPositions = new List<Vector2Int>
    {
        new Vector2Int(0, 0), // ���� ��
        new Vector2Int(0, 3),    // ���� �Ʒ�
        new Vector2Int(3, 3),   // ������ �Ʒ�
        new Vector2Int(3, 0),   // ������ ��
        
    };

    Vector2Int[] directions = new Vector2Int[] // ���� �迭
    {
            new Vector2Int(0, -1),  // ��
            new Vector2Int(0, 1), // �Ʒ�
            new Vector2Int(-1, 0), // ����
            new Vector2Int(1, 0)   // ������
    };


    private void Start()
    {
        // ���� ����
        GenerateMap();
    }

    // ���� ���̾ƿ��� �����ϴ� �Լ�
    private void GenerateMap()
    {
        // �׸��� �ʱ�ȭ (4x4 �� ����)
        for (int y = 0; y < gridSize; y++)
        {
            Row2 row = new Row2();
            for (int x = 0; x < gridSize; x++)
            {
                Cell2 cell = new Cell2();
                row.cells.Add(cell);
            }
            mapRows.Add(row);
        }

        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>(); // �̹� ��ġ�� ���� ����

        // ������ ��ġ�� �������� ����
        Vector2Int startPos = StartEndPos(out Vector2Int bossPos);
        player_Pos = startPos;

        PlaceRoom(startPos, startPrefab);

        
        occupiedPositions.Add(startPos); // ���� ���� �̹� ��ġ�� ������ ����

        // ���� �濡�� ���������� ���� ��θ� ���� (�̷�ó��)
        List<Vector2Int> pathToBoss = GenerateComplexPath(player_Pos, bossPos);

        // ��ο� �ִ� ����� ��ġ (�����ϰ�)
        foreach (var pathPos in pathToBoss)
        {
            if (pathPos == bossPos)
            {
                // ������ ��ġ
                PlaceRoom(pathPos, bossRoomPrefab, isBoss: true);
                occupiedPositions.Add(pathPos);
            }
            else if (pathPos != startPos && !occupiedPositions.Contains(pathPos))  // ���� �濡�� �̹� ���� ��ġ�Ǿ� �����Ƿ� ����
            {
                // ��� ������ ������ �� ��ġ
                GameObject randomRoom = GetRandomRoomPrefab();
                PlaceRoom(pathPos, randomRoom);
                occupiedPositions.Add(pathPos);
            }
        }

        // ��θ� ������ �� ����
        PlaceMandatoryRooms(pathToBoss, bossPos);

    }

    // ���۹�� ������ ���ϴ� �޼���
    private Vector2Int StartEndPos(out Vector2Int bossPos)
    {
        // ���� �� ��ġ�� ����
        int startRoomIndex = Random.Range(0, availableBossPositions.Count);
        Vector2Int startPos = availableBossPositions[startRoomIndex];

        // ���� �� ��ġ�� �ִ� ������ ��ǥ�� ���
        bossPos = GetFurthestCorner(startPos);

        return startPos;
    }

    // �־��� ���� �濡�� ���� �� �ڳ� ���� ��ȯ�ϴ� �Լ� (����ư �Ÿ� ���)
    private Vector2Int GetFurthestCorner(Vector2Int startPos)
    {
        Vector2Int furthestPos = Vector2Int.zero;  // �⺻��
        int maxDistance = -1;  // ��������� �ִ� �Ÿ�

        // ��� �ڳ� �� �߿��� ���� �� ��ǥ ã��
        foreach (Vector2Int cornerPos in availableBossPositions)
        {
            int distance = Mathf.Abs(startPos.x - cornerPos.x) + Mathf.Abs(startPos.y - cornerPos.y); // ����ư �Ÿ� ���
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPos = cornerPos;
            }
        }

        return furthestPos;
    }

    // ���� �濡�� ��������� ���� ������ ��θ� �����ϴ� �Լ�
    private List<Vector2Int> GenerateComplexPath(Vector2Int startPos, Vector2Int endPos)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPos = startPos;
        path.Add(currentPos);

        while (currentPos != endPos)
        {
            List<Vector2Int> possibleDirections = new List<Vector2Int>();

            // ���� �̵��� �ʿ��ϴٸ� ���� ���� �߰�
            if (currentPos.x != endPos.x)
            {
                if (currentPos.x < endPos.x)
                    possibleDirections.Add(Vector2Int.right);
                else
                    possibleDirections.Add(Vector2Int.left);
            }

            // ���� �̵��� �ʿ��ϴٸ� ���� ���� �߰�
            if (currentPos.y != endPos.y)
            {
                if (currentPos.y < endPos.y)
                    possibleDirections.Add(Vector2Int.up);
                else
                    possibleDirections.Add(Vector2Int.down);
            }

            // �������� �� �� �� ���ư��� ��� �߰� (��θ� ���� ����)
            if (Random.Range(0, 10) > 7) // 30% Ȯ���� ��θ� ��
            {
                if (currentPos.x > 0)
                    possibleDirections.Add(Vector2Int.left);
                if (currentPos.x < gridSize - 1)
                    possibleDirections.Add(Vector2Int.right);
                if (currentPos.y > 0)
                    possibleDirections.Add(Vector2Int.down);
                if (currentPos.y < gridSize - 1)
                    possibleDirections.Add(Vector2Int.up);
            }

            // �������� ���� ����
            Vector2Int randomDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

            // ������ �������� �̵�
            currentPos += randomDirection;
            path.Add(currentPos);
        }

        return path;
    }

    // �ּ� �� ������ �����ϴ� ����� ��ġ�ϴ� �Լ�
    private void PlaceMandatoryRooms(List<Vector2Int> pathToBoss, Vector2Int bossPos)
    {
        List<Vector2Int> availablePositions = GetAvailablePositions(pathToBoss, bossPos);

        // ���� �� ��ġ
        for (int i = 0; i < maxBlockedRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            PlaceRoom(randomPos, blockedRoomPrefab, isBlocked: true);
        }

        // ���� �� ��ġ
        for (int i = 0; i < minChestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            PlaceRoom(randomPos, chestRoomPrefab);
        }

        // �޽� ��
        for (int i = 0; i < minRestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            PlaceRoom(randomPos, restRoomPrefab);
        }

        for (int i = 0; i < minBattleRooms; i++) 
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            GameObject battleRoom = battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Count())];
            PlaceRoom(randomPos, battleRoom);
        }


        while (availablePositions.Count > 0)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            GameObject randomRoom = GetRandomRoomPrefab();
            PlaceRoom(randomPos, randomRoom);
        }
    }


    // ��ο� �������� �������� �ʴ� �� �ڸ��� ��ȯ�ϴ� �Լ�
    private List<Vector2Int> GetAvailablePositions(List<Vector2Int> pathToBoss, Vector2Int bossPos)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                // ������� ��δ� ����
                if (pos != bossPos && !pathToBoss.Contains(pos) && pos != player_Pos)
                {
                    availablePositions.Add(pos);
                }
            }
        }

        return availablePositions;
    }

    // ��� ������ ��ġ �� �ϳ��� �������� �����ϰ� �ش� ��ġ�� �����ϴ� �Լ�
    private Vector2Int GetRandomPosition(List<Vector2Int> availablePositions)
    {
        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2Int pos = availablePositions[randomIndex];
        availablePositions.RemoveAt(randomIndex);
        return pos;
    }

    // 
    private GameObject GetRandomRoomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < 70)
        {
            return battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)];
        }
        else if (randomValue < 80)
        {
            return chestRoomPrefab;
        }
        else
        {
            return restRoomPrefab;
        }
    }

    // ���� Ư�� ��ġ�� ��ġ�ϰ� ���� �� �÷��� �����ϴ� �Լ�
    private void PlaceRoom(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        Cell2 cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomPrefab;
        cell.isBoss = isBoss;
        cell.isBlocked = isBlocked;  // ���� ���̸� true, �ƴϸ� false

        // ���忡 ���� ���� (��ġ ���)
        Vector3 roomPos = new Vector3(pos.x * roomSpacing, pos.y * -roomSpacing, 0);
        Instantiate(roomPrefab, roomPos, Quaternion.identity);
    }
}
