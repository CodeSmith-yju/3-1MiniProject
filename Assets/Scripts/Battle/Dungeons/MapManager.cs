using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public List<Cell> cells = new(); // �� ���� ��(��) ����Ʈ
}


public class MapManager : MonoBehaviour
{
    public List<Row> mapRows = new List<Row>();
    public NavMeshSurface nav_Surface;

    [Header("Map_Prefabs")]
    public GameObject startPrefab;
    public GameObject[] battleRoomPrefabs;    // ������ ������
    public GameObject chestRoomPrefab;        // ���ڹ� ������
    public GameObject restRoomPrefab;         // �޽Ĺ� ������
    public GameObject blockedRoomPrefab;      // ���� �� ������
    public GameObject bossRoomPrefab;         // ���� �� ������


    Vector2Int player_Pos;             // �÷��̾� ���� ��ġ
    int gridSize = 4; // �׸��� ũ��
    int roomSpacing = 25;              // �� ����
    // �ּ�/�ִ� �� ����
    int maxBlockedRooms = 4;
    int minChestRooms = 1;
    int minRestRooms = 1;
    int minBattleRooms = 3;

    private Vector2Int select_Direction;
    public Transform cur_Room;

    [Header("MiniMaps")]
    public Camera map_Camera;
    public Camera map_Camera_Big;
    [SerializeField] GameObject map_Mark_Prefab;
    [SerializeField] Transform map_Tf;


    public bool isMoveDone = false;
    [SerializeField] List<GameObject> map_Icon; // 0 : ��, 1 : �Ʒ�, 2 : ����, 3 : ������


    private List<Vector2Int> random_Start_Pos = new List<Vector2Int>
    {
        new Vector2Int(0, 0),   // �»��
        new Vector2Int(0, 3),   // ���ϴ�
        new Vector2Int(3, 0),   // ����
        new Vector2Int(3, 3),   // ���ϴ�
    };

    Vector2Int[] directions = new Vector2Int[] // ���� �迭
    {
            new Vector2Int(0, -1),  // ��
            new Vector2Int(0, 1), // �Ʒ�
            new Vector2Int(-1, 0), // ����
            new Vector2Int(1, 0)   // ������
    };



    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        if (mapRows == null)
            mapRows = new();


        gridSize += SetLevel(GameUiMgr.single.dungeon_Level);
        maxBlockedRooms += SetLevel(GameUiMgr.single.dungeon_Level);
        minChestRooms += SetLevel(GameUiMgr.single.dungeon_Level);
        minRestRooms += SetLevel(GameUiMgr.single.dungeon_Level);
        minBattleRooms += SetLevel(GameUiMgr.single.dungeon_Level);

        if (BattleManager.Instance.dialogue != null && BattleManager.Instance.dialogue.isTutorial)
        {
            player_Pos = new Vector2Int(0, 0);
            SetRoom(player_Pos);
            MiniMapUpdate(player_Pos);
            BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start); // ù�� ����
        }
        else
        {
            StartCoroutine(GenerateDungeon());
        }    
    }

    // ���̵��� ���� ��ġ ����
    public int SetLevel(int level)
    {
        int set_Level;

        switch (level)
        {
            case 2:
                set_Level = 1;
                return set_Level;
            default:
                return 0;
        }
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

                nav_Surface.BuildNavMeshAsync();
                SetRoom(player_Pos);
                MiniMapUpdate(player_Pos);

                BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start); // ù�� ����

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
                Row row = new Row();
                for (int x = 0; x < gridSize; x++)
                {
                    Cell cell = new Cell();
                    row.cells.Add(cell);
                }
                mapRows.Add(row);
            }
        }
        // �������� �̸� �Ҵ�
        Vector2Int startPos = random_Start_Pos[Random.Range(0, random_Start_Pos.Count)];
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
        Cell bossRoomCell = mapRows[bossRoomPos.y].cells[bossRoomPos.x];

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



    // ������ �ʱ�ȭ�ϴ� �Լ� (���� �� ����)
    private void ResetDungeon()
    {
        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
            {
                if (cell.cellObject != null)
                {
                    cell.cellObject = null;
                    cell.isBlocked = false;
                    cell.isBoss = false;
                    cell.isClear = false;
                }
            }
        }
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

        if (randomValue < 85)  // 85% Ȯ���� ���� �� ��ġ
        {
            return battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)];
        }
        else if (randomValue < 90)  // 5% Ȯ���� ���� ��
        {
            return chestRoomPrefab;
        }
        else  // 10% Ȯ���� �޽� ��
        {
            return restRoomPrefab;
        }
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
        if (mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj != null)
        {
            mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.SetActive(true);
            mapRows[cur_Room.y].cells[cur_Room.x].isVisit = true; // ����� �湮
            mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.white; // ����� ������ �Ͼ������ ����

            map_Camera.transform.position =
                new Vector3(mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.transform.position.x,
                mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.transform.position.y,
                map_Camera.transform.position.z);
        } 
       
        for (int i = 0; i < directions.Length; i++) 
        {
            Vector2Int direction = directions[i];
            Vector2Int newPos = cur_Room + direction;

            
            if (IsValid(newPos) && !IsBlock(newPos))
            {
                if (mapRows[newPos.y].cells[newPos.x].minimap_Obj != null)
                {
                    mapRows[newPos.y].cells[newPos.x].minimap_Obj.SetActive(true); // �����¿�� ������ ��
                    mapRows[newPos.y].cells[newPos.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.gray; // �����¿�� ������ ���� ȸ������ ����
                }
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
                    if (cell.minimap_Obj != null)
                        cell.minimap_Obj.SetActive(false);
                }
                else
                {
                    cell.cellObject.SetActive(true);
                    cur_Room = cell.cellObject.transform;
                    Camera.main.transform.position = new Vector3(cur_Room.position.x, cur_Room.position.y, Camera.main.transform.position.z);
                }
            }
        }
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

                if (IsValid(neighborPos) && !IsBlock(neighborPos) && !distances.ContainsKey(neighborPos))
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
                if (IsValid(neighborPos) && !IsBlock(neighborPos) && !connectedRooms.Contains(neighborPos))
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
        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
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

    // Cell ��ü�� ��ǥ�� ã�� ��ȯ�ϴ� �Լ�
    private Vector2Int GetCellPosition(Cell cell)
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

    // �˾����� Ȯ�� ��ư ���� �� ����ϴ� �� �̵� �޼���
    public void ConfirmRoom()
    {
        MoveToRoom(select_Direction);
    }

    // ������ �� ���� �� ������ �ӽ÷� �Ҵ�
    private void SetRoomValue(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        // ���� ���� ������ ������Ʈ�� ����
        Cell cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomPrefab;
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
    }

    // ���� �Ҵ�� ������ �������� �� ����
    private void PlaceAllRooms()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Cell cell = mapRows[y].cells[x];

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
        Cell cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomInstance;  // ������ �ν��Ͻ��� ����
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
        cell.minimap_Obj = PlaceMinimap(map_Tf, map_Mark_Prefab, pos, isBlocked);
        cell.isVisit = isVisit;
    }

    // ������ ���缭 �̴ϸ� ���� �޼���
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
        if (!isMoveDone && BattleManager.Instance.ui.out_Portal.activeSelf)
            BattleManager.Instance.ui.out_Portal.GetComponent<FadeEffect>().fadein = true;

    }

    // �� �̵� �� �� ī�޶�� �̴ϸ� ī�޶� �̵� �޼���
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
                    if (cell.minimap_Obj != null)
                        cell.minimap_Obj.SetActive(true);
                }
                else
                {
                    cell.cellObject.SetActive(false);
                    if (cell.minimap_Obj != null) 
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



