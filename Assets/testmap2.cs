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
    public GameObject[] battleRoomPrefabs;    // 전투방 프리팹
    public GameObject chestRoomPrefab;        // 상자방 프리팹
    public GameObject restRoomPrefab;         // 휴식방 프리팹
    public GameObject blockedRoomPrefab;      // 막힌 방 프리팹
    public GameObject bossRoomPrefab;         // 보스 방 프리팹


    Vector2Int player_Pos;             // 플레이어 시작 위치
    int gridSize = 4;                  // 4x4 그리드 크기
    int roomSpacing = 25;              // 방 간격

    // 최소/최대 방 개수
    int maxBlockedRooms = 4;
    int minChestRooms = 1;
    int minRestRooms = 1;
    int minBattleRooms = 3;

    [SerializeField] GameObject map_Mark_Prefab;
    [SerializeField] Transform map_Tf;

    private List<Vector2Int> availableStartPositions = new List<Vector2Int>
    {
        new Vector2Int(0, 0),   // 좌상단
        new Vector2Int(0, 3),   // 좌하단
        new Vector2Int(3, 0),   // 우상단
        new Vector2Int(3, 3),   // 우하단
    };

    Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, -1),  // 위쪽
        new Vector2Int(0, 1),   // 아래쪽
        new Vector2Int(-1, 0),  // 왼쪽
        new Vector2Int(1, 0)    // 오른쪽
    };

    private void Start()
    {
        StartCoroutine(GenerateDungeon());
    }

    // 던전 생성 함수
    private IEnumerator GenerateDungeon()
    {
        bool allRoomsConnect = false;


        while (!allRoomsConnect)
        {
            // 던전 초기화
            ResetDungeon();

            // 그리드 생성 및 생성된 그리드에 방 정보 할당
            GenerateDungeonGrid();

            yield return null;

            // 고립된 방이 없는지 체크
            allRoomsConnect = AreAllRoomsConnected(player_Pos);

            // 다 연결되어 있으면
            if (allRoomsConnect)
            {
                PlaceAllRooms();
                Debug.Log("던전 생성");
                break;
            }

            yield return null;
        }
        
        yield break;
    }

    // 그리드를 생성하고 그리드에 랜덤으로 방의 정보를 할당
    private void GenerateDungeonGrid()
    {
        // 4x4 그리드를 초기화하고 방 배치

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
        // 시작지점 미리 할당
        Vector2Int startPos = availableStartPositions[Random.Range(0, availableStartPositions.Count)];
        player_Pos = startPos;
        SetRoomValue(player_Pos, startPrefab);

        // 이후 필요한 방들 배치 (필수 방 및 나머지 방들)
        PlaceMandatoryRooms();
    }

    // 필수 방들을 배치하는 함수 (막힌 방, 상자 방, 휴식 방, 전투 방)
    private void PlaceMandatoryRooms()
    {
        List<Vector2Int> availablePositions = GetAvailablePositions();

        // 막힌 방 4개 배치
        for (int i = 0; i < maxBlockedRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, blockedRoomPrefab, isBlocked: true);
        }

        // 상자 방 1개 배치
        for (int i = 0; i < minChestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, chestRoomPrefab);
        }

        // 휴식 방 1개 배치
        for (int i = 0; i < minRestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            SetRoomValue(randomPos, restRoomPrefab);
        }

        // 전투 방 3개 배치
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

        // 보스방 배치 체크 (상자방이나 휴식방의 최소 갯수에 맞추도록)
        ReassignRooms(availablePositions);
    }

    // 보스방을 배치할 때, 상자방이나 휴식방이 덮어씌워지면 해당 방을 남은 빈 방에 재배치
    private void ReassignRooms(List<Vector2Int> availablePositions)
    {
        // 보스방 위치를 계산
        Vector2Int bossRoomPos = FindFurthestRoom(player_Pos);
        Cell3 bossRoomCell = mapRows[bossRoomPos.y].cells[bossRoomPos.x];

        // 보스방이 이미 상자방이나 휴식방을 덮어쓰는 경우
        if (bossRoomCell.cellObject != null)
        {
            // 상자방이나 휴식방을 남은 빈 방으로 재배치
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
                // 상자방이나 휴식방이 아니면 랜덤한 방으로 빈 방 채우기
                if (availablePositions.Count > 0)
                {
                    Vector2Int randomPos = GetRandomPosition(availablePositions);
                    GameObject randomRoom = GetRandomRoomPrefab();
                    SetRoomValue(randomPos, randomRoom);
                }
            }
        }

        // 보스방 배치
        SetRoomValue(bossRoomPos, bossRoomPrefab, isBoss: true);
    }

    // 시작 방에서 가장 먼 방을 찾는 함수 (BFS 알고리즘 사용)
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

    // BFS를 사용하여 시작 방에서 연결된 모든 방을 찾는 함수
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

                // 그리드 내에 있고, 막힌 방이 아니며 아직 연결되지 않은 경우 탐색
                if (IsWithinGrid(neighborPos) && !IsBlockedRoom(neighborPos) && !connectedRooms.Contains(neighborPos))
                {
                    queue.Enqueue(neighborPos);
                    connectedRooms.Add(neighborPos);
                }
            }
        }

        return connectedRooms; // 연결된 방들의 좌표 반환
    }

        // 모든 방들이 연결되었는지 확인하는 함수
        private bool AreAllRoomsConnected(Vector2Int startRoomPos)
        {
            // 시작 방에서 연결된 방들의 좌표를 얻음
            HashSet<Vector2Int> connectedRooms = FindConnectedRooms(startRoomPos);

            // 그리드 내의 모든 배치된 방을 확인
            foreach (Row3 row in mapRows)
            {
                foreach (Cell3 cell in row.cells)
                {
                    Vector2Int pos = GetCellPosition(cell);

                    // 방이 배치되어 있고, 막힌 방이 아니며 연결된 방들에 포함되지 않았다면
                    if (!cell.isBlocked && cell.cellObject != null && !connectedRooms.Contains(pos))
                    {
                        return false;  // 연결되지 않은 방이 있으면 false 반환
                    }
                }
            }

            return true;  // 모든 방이 연결되었을 때 true 반환
        }

    // Cell3 객체의 좌표를 찾아 반환하는 함수
    private Vector2Int GetCellPosition(Cell3 cell)
    {
        for (int y = 0; y < mapRows.Count; y++)
        {
            for (int x = 0; x < mapRows[y].cells.Count; x++)
            {
                // 해당 좌표의 셀이 현재 찾는 셀과 일치하는지 확인
                if (mapRows[y].cells[x] == cell)
                {
                    // 셀이 위치한 좌표를 반환 (x는 열, y는 행)
                    return new Vector2Int(x, y);
                }
            }
        }

        // 셀을 찾을 수 없을 경우 기본값 반환 (찾을 수 없는 경우는 거의 없도록)
        return Vector2Int.zero;
    }

    private void SetRoomValue(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        // 현재 셀에 복제된 오브젝트를 저장
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

                if (cell.cellObject != null)  // 정보가 저장된 방만 처리
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
                        GameObject roomPrefab = cell.cellObject;  // 미리 저장된 방 프리팹 사용
                        PlaceRoom(pos, roomPrefab);
                    }
                }
            }
        }
    }


    // 방을 배치하는 함수
    private void PlaceRoom(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false, bool isVisit = false)
    {
        // 방 오브젝트를 복제하여 씬에 배치하고, 그 인스턴스를 cellObject로 저장
        GameObject roomInstance = Instantiate(roomPrefab, new Vector3(pos.x * roomSpacing, pos.y * -roomSpacing, 0), Quaternion.identity);
        

        // 현재 셀에 복제된 오브젝트를 저장
        Cell3 cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomInstance;  // 복제된 인스턴스를 저장
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
        cell.minimap_Obj = PlaceMinimap(map_Tf, map_Mark_Prefab, pos, isBlocked);
        cell.isVisit = isVisit;
    }

    private GameObject PlaceMinimap(Transform parent, GameObject prefab, Vector2Int pos, bool isBlocked) 
    {
        if (!isBlocked)
        {
            // 미니맵 생성 및 부모 설정
            GameObject miniMaps = Instantiate(prefab);

            // miniMaps를 map_Tf의 자식으로 설정
            miniMaps.transform.SetParent(parent);

            // 부모의 위치를 기준으로 간격을 적용 (로컬 위치로 설정)
            miniMaps.transform.localPosition = new Vector3(pos.x * 7, pos.y * -7, 0);

            // 각도를 유지하기 위해 로컬 회전 값을 초기화
            miniMaps.transform.localRotation = Quaternion.identity;

            return miniMaps;
        }

        return null;
    }

    // 던전을 초기화하는 함수 (기존 방 삭제)
    private void ResetDungeon()
    {
        foreach (Row3 row in mapRows)
        {
            foreach (Cell3 cell in row.cells)
            {
                if (cell.cellObject != null)
                {
                    Destroy(cell.cellObject);  // 기존 방 오브젝트 삭제
                    cell.cellObject = null;
                    cell.isBlocked = false;
                    cell.isBoss = false;
                    cell.isClear = false;
                }
            }
        }

        //mapRows.Clear();
    }


    // 그리드 내에 있는지 확인하는 함수
    private bool IsWithinGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    // 방이 막힌 방인지 확인하는 함수
    private bool IsBlockedRoom(Vector2Int pos)
    {
        return mapRows[pos.y].cells[pos.x].isBlocked;
    }

    // 방을 무작위로 선택하는 함수
    private Vector2Int GetRandomPosition(List<Vector2Int> availablePositions)
    {
        int randomIndex = Random.Range(0, availablePositions.Count);
        Vector2Int pos = availablePositions[randomIndex];
        availablePositions.RemoveAt(randomIndex);
        return pos;
    }

    // 사용 가능한 방 위치를 반환하는 함수
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

    // 랜덤으로 방 종류를 선택하는 함수 (막힌 방 제외)
    private GameObject GetRandomRoomPrefab()
    {
        int randomValue = Random.Range(0, 100);

        if (randomValue < 80)  // 80% 확률로 전투 방 배치
        {
            return battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)];
        }
        else if (randomValue < 90)  // 10% 확률로 상자 방
        {
            return chestRoomPrefab;
        }
        else  // 10% 확률로 휴식 방
        {
            return restRoomPrefab;
        }
    }
}
