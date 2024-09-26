using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Cell3
{
    public GameObject cellObject;
    public bool isClear;
    public bool isBlocked;
    public bool isBoss;

    [Header("Minimap")]
    public GameObject minimap_Obj;
    public bool isVisit;
}

[System.Serializable]
public class Row3
{
    public List<Cell3> cells = new List<Cell3>();
}

public class testmap2 : MonoBehaviour
{
    [Header("Map_List")]
    public List<Row3> mapRows = new List<Row3>();

    [Header("Map_Prefabs")]
    public GameObject startPrefab;
    public GameObject[] battleRoomPrefabs;    // ������ ������
    public GameObject chestRoomPrefab;        // ���ڹ� ������
    public GameObject restRoomPrefab;         // �޽Ĺ� ������
    public GameObject blockedRoomPrefab;      // ���� �� ������
    public GameObject bossRoomPrefab;         // ���� �� ������


    Vector2Int player_Pos;             // �÷��̾� ���� ��ġ
    int gridSize = 4;                  // 4x4 �׸��� ũ��
    int roomSpacing = 25;              // �� ����

    // �ּ�/�ִ� �� ����
    int maxBlockedRooms = 4;
    int minChestRooms = 1;
    int minRestRooms = 1;
    int minBattleRooms = 3;

    [SerializeField] GameObject map_Mark_Prefab;
    [SerializeField] Transform map_Tf;

    private List<Vector2Int> availableStartPositions = new List<Vector2Int>
    {
        new Vector2Int(0, 0),   // �»��
        new Vector2Int(0, 3),   // ���ϴ�
        new Vector2Int(3, 0),   // ����
        new Vector2Int(3, 3),   // ���ϴ�
    };

    Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, -1),  // ����
        new Vector2Int(0, 1),   // �Ʒ���
        new Vector2Int(-1, 0),  // ����
        new Vector2Int(1, 0)    // ������
    };

    private void Start()
    {
        StartCoroutine(GenerateDungeon());
    }

    // ���� ���� �Լ�
    private IEnumerator GenerateDungeon()
    {
        bool allRoomsConnect = false;


        while (!allRoomsConnect)
        {
            // ���� �ʱ�ȭ
            ResetDungeon();

            // �׸��� ���� �� ������ �׸��忡 �� ���� �Ҵ�
            GenerateDungeonGrid();

            yield return null;

            // ���� ���� ������ üũ
            allRoomsConnect = AreAllRoomsConnected(player_Pos);

            // �� ����Ǿ� ������
            if (allRoomsConnect)
            {
                PlaceAllRooms();
                Debug.Log("���� ����");
                break;
            }

            yield return null;
        }
        
        yield break;
    }

    // �׸��带 �����ϰ� �׸��忡 �������� ���� ������ �Ҵ�
    private void GenerateDungeonGrid()
    {
        // 4x4 �׸��带 �ʱ�ȭ�ϰ� �� ��ġ

        if (mapRows.Count <= 0)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Row3 row = new Row3();
                for (int x = 0; x < gridSize; x++)
                {
                    Cell3 cell = new Cell3();
                    row.cells.Add(cell);
                }
                mapRows.Add(row);
            }
        }
        // �������� �̸� �Ҵ�
        Vector2Int startPos = availableStartPositions[Random.Range(0, availableStartPositions.Count)];
        player_Pos = startPos;
        SetRoomValue(player_Pos, startPrefab);

        // ���� �ʿ��� ��� ��ġ (�ʼ� �� �� ������ ���)
        PlaceMandatoryRooms();
    }

    // �ʼ� ����� ��ġ�ϴ� �Լ� (���� ��, ���� ��, �޽� ��, ���� ��)
    private void PlaceMandatoryRooms()
    {
        List<Vector2Int> availablePositions = GetAvailablePositions();

        // ���� �� 4�� ��ġ
        for (int i = 0; i < maxBlockedRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, blockedRoomPrefab, isBlocked: true);
        }

        // ���� �� 1�� ��ġ
        for (int i = 0; i < minChestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, chestRoomPrefab);
        }

        // �޽� �� 1�� ��ġ
        for (int i = 0; i < minRestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, restRoomPrefab);
        }

        // ���� �� 3�� ��ġ
        for (int i = 0; i < minBattleRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)]);
        }

        int reservedEmptyRooms = 1;

        while (availablePositions.Count > reservedEmptyRooms)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            GameObject randomRoom = GetRandomRoomPrefab();
            SetRoomValue(randomPos, randomRoom);
        }

        // ������ ��ġ üũ (���ڹ��̳� �޽Ĺ��� �ּ� ������ ���ߵ���)
        ReassignRooms(availablePositions);
    }

    // �������� ��ġ�� ��, ���ڹ��̳� �޽Ĺ��� ��������� �ش� ���� ���� �� �濡 ���ġ
    private void ReassignRooms(List<Vector2Int> availablePositions)
    {
        // ������ ��ġ�� ���
        Vector2Int bossRoomPos = FindFurthestRoom(player_Pos);
        Cell3 bossRoomCell = mapRows[bossRoomPos.y].cells[bossRoomPos.x];

        // �������� �̹� ���ڹ��̳� �޽Ĺ��� ����� ���
        if (bossRoomCell.cellObject != null)
        {
            // ���ڹ��̳� �޽Ĺ��� ���� �� ������ ���ġ
            if (bossRoomCell.cellObject.name.Contains(chestRoomPrefab.name))
            {
                if (availablePositions.Count > 0)
                {
                    Vector2Int randomPos = GetRandomPosition(availablePositions);
                    SetRoomValue(randomPos, chestRoomPrefab);
                }
            }
            else if (bossRoomCell.cellObject.name.Contains(restRoomPrefab.name))
            {
                if (availablePositions.Count > 0)
                {
                    Vector2Int randomPos = GetRandomPosition(availablePositions);
                    SetRoomValue(randomPos, restRoomPrefab);
                }
            }
            else
            {
                // ���ڹ��̳� �޽Ĺ��� �ƴϸ� ������ ������ �� �� ä���
                if (availablePositions.Count > 0)
                {
                    Vector2Int randomPos = GetRandomPosition(availablePositions);
                    GameObject randomRoom = GetRandomRoomPrefab();
                    SetRoomValue(randomPos, randomRoom);
                }
            }
        }

        // ������ ��ġ
        SetRoomValue(bossRoomPos, bossRoomPrefab, isBoss: true);
    }

    // ���� �濡�� ���� �� ���� ã�� �Լ� (BFS �˰��� ���)
    private Vector2Int FindFurthestRoom(Vector2Int startPos)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> distances = new Dictionary<Vector2Int, int>();
        queue.Enqueue(startPos);
        distances[startPos] = 0;

        Vector2Int furthestRoom = startPos;
        int maxDistance = 0;

        while (queue.Count > 0)
        {
            Vector2Int currentRoom = queue.Dequeue();
            int currentDistance = distances[currentRoom];

            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighborPos = currentRoom + direction;

                if (IsWithinGrid(neighborPos) && !IsBlockedRoom(neighborPos) && !distances.ContainsKey(neighborPos))
                {
                    distances[neighborPos] = currentDistance + 1;
                    queue.Enqueue(neighborPos);

                    if (distances[neighborPos] > maxDistance)
                    {
                        maxDistance = distances[neighborPos];
                        furthestRoom = neighborPos;
                    }
                }
            }
        }

        return furthestRoom;
    }

    // BFS�� ����Ͽ� ���� �濡�� ����� ��� ���� ã�� �Լ�
    private HashSet<Vector2Int> FindConnectedRooms(Vector2Int startRoomPos)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> connectedRooms = new HashSet<Vector2Int>();

        queue.Enqueue(startRoomPos);
        connectedRooms.Add(startRoomPos);

        while (queue.Count > 0)
        {
            Vector2Int currentRoom = queue.Dequeue();

            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighborPos = currentRoom + direction;

                // �׸��� ���� �ְ�, ���� ���� �ƴϸ� ���� ������� ���� ��� Ž��
                if (IsWithinGrid(neighborPos) && !IsBlockedRoom(neighborPos) && !connectedRooms.Contains(neighborPos))
                {
                    queue.Enqueue(neighborPos);
                    connectedRooms.Add(neighborPos);
                }
            }
        }

        return connectedRooms; // ����� ����� ��ǥ ��ȯ
    }

        // ��� ����� ����Ǿ����� Ȯ���ϴ� �Լ�
        private bool AreAllRoomsConnected(Vector2Int startRoomPos)
        {
            // ���� �濡�� ����� ����� ��ǥ�� ����
            HashSet<Vector2Int> connectedRooms = FindConnectedRooms(startRoomPos);

            // �׸��� ���� ��� ��ġ�� ���� Ȯ��
            foreach (Row3 row in mapRows)
            {
                foreach (Cell3 cell in row.cells)
                {
                    Vector2Int pos = GetCellPosition(cell);

                    // ���� ��ġ�Ǿ� �ְ�, ���� ���� �ƴϸ� ����� ��鿡 ���Ե��� �ʾҴٸ�
                    if (!cell.isBlocked && cell.cellObject != null && !connectedRooms.Contains(pos))
                    {
                        return false;  // ������� ���� ���� ������ false ��ȯ
                    }
                }
            }

            return true;  // ��� ���� ����Ǿ��� �� true ��ȯ
        }

    // Cell3 ��ü�� ��ǥ�� ã�� ��ȯ�ϴ� �Լ�
    private Vector2Int GetCellPosition(Cell3 cell)
    {
        for (int y = 0; y < mapRows.Count; y++)
        {
            for (int x = 0; x < mapRows[y].cells.Count; x++)
            {
                // �ش� ��ǥ�� ���� ���� ã�� ���� ��ġ�ϴ��� Ȯ��
                if (mapRows[y].cells[x] == cell)
                {
                    // ���� ��ġ�� ��ǥ�� ��ȯ (x�� ��, y�� ��)
                    return new Vector2Int(x, y);
                }
            }
        }

        // ���� ã�� �� ���� ��� �⺻�� ��ȯ (ã�� �� ���� ���� ���� ������)
        return Vector2Int.zero;
    }

    private void SetRoomValue(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        // ���� ���� ������ ������Ʈ�� ����
        Cell3 cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomPrefab;
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
    }

    private void PlaceAllRooms()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Cell3 cell = mapRows[y].cells[x];

                if (cell.cellObject != null)  // ������ ����� �游 ó��
                {
                    if (cell.isBlocked)
                    {
                        PlaceRoom(pos, blockedRoomPrefab, isBlocked: true);
                    }
                    else if (cell.isBoss)
                    {
                        PlaceRoom(pos, bossRoomPrefab, isBoss: true);
                    }
                    else
                    {
                        GameObject roomPrefab = cell.cellObject;  // �̸� ����� �� ������ ���
                        PlaceRoom(pos, roomPrefab);
                    }
                }
            }
        }
    }


    // ���� ��ġ�ϴ� �Լ�
    private void PlaceRoom(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false, bool isVisit = false)
    {
        // �� ������Ʈ�� �����Ͽ� ���� ��ġ�ϰ�, �� �ν��Ͻ��� cellObject�� ����
        GameObject roomInstance = Instantiate(roomPrefab, new Vector3(pos.x * roomSpacing, pos.y * -roomSpacing, 0), Quaternion.identity);
        

        // ���� ���� ������ ������Ʈ�� ����
        Cell3 cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomInstance;  // ������ �ν��Ͻ��� ����
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
        cell.minimap_Obj = PlaceMinimap(map_Tf, map_Mark_Prefab, pos, isBlocked);
        cell.isVisit = isVisit;
    }

    private GameObject PlaceMinimap(Transform parent, GameObject prefab, Vector2Int pos, bool isBlocked) 
    {
        if (!isBlocked)
        {
            // �̴ϸ� ���� �� �θ� ����
            GameObject miniMaps = Instantiate(prefab);

            // miniMaps�� map_Tf�� �ڽ����� ����
            miniMaps.transform.SetParent(parent);

            // �θ��� ��ġ�� �������� ������ ���� (���� ��ġ�� ����)
            miniMaps.transform.localPosition = new Vector3(pos.x * 7, pos.y * -7, 0);

            // ������ �����ϱ� ���� ���� ȸ�� ���� �ʱ�ȭ
            miniMaps.transform.localRotation = Quaternion.identity;

            return miniMaps;
        }

        return null;
    }

    // ������ �ʱ�ȭ�ϴ� �Լ� (���� �� ����)
    private void ResetDungeon()
    {
        foreach (Row3 row in mapRows)
        {
            foreach (Cell3 cell in row.cells)
            {
                if (cell.cellObject != null)
                {
                    Destroy(cell.cellObject);  // ���� �� ������Ʈ ����
                    cell.cellObject = null;
                    cell.isBlocked = false;
                    cell.isBoss = false;
                    cell.isClear = false;
                }
            }
        }

        //mapRows.Clear();
    }


    // �׸��� ���� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsWithinGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    // ���� ���� ������ Ȯ���ϴ� �Լ�
    private bool IsBlockedRoom(Vector2Int pos)
    {
        return mapRows[pos.y].cells[pos.x].isBlocked;
    }

    // ���� �������� �����ϴ� �Լ�
    private Vector2Int GetRandomPosition(List<Vector2Int> availablePositions)
    {
        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2Int pos = availablePositions[randomIndex];
        availablePositions.RemoveAt(randomIndex);
        return pos;
    }

    // ��� ������ �� ��ġ�� ��ȯ�ϴ� �Լ�
    private List<Vector2Int> GetAvailablePositions()
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (mapRows[y].cells[x].cellObject == null)
                {
                    availablePositions.Add(pos);
                }
            }
        }

        return availablePositions;
    }

    // �������� �� ������ �����ϴ� �Լ� (���� �� ����)
    private GameObject GetRandomRoomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < 80)  // 80% Ȯ���� ���� �� ��ġ
        {
            return battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)];
        }
        else if (randomValue < 90)  // 10% Ȯ���� ���� ��
        {
            return chestRoomPrefab;
        }
        else  // 10% Ȯ���� �޽� ��
        {
            return restRoomPrefab;
        }
    }
}
