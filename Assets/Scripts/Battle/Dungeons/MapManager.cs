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
    public List<Cell> cells = new(); // 한 행의 셀(방) 리스트
}


public class MapManager : MonoBehaviour
{
    public List<Row> mapRows = new List<Row>();
    public NavMeshSurface nav_Surface;

    [Header("Map_Prefabs")]
    public GameObject startPrefab;
    public GameObject[] battleRoomPrefabs;    // 전투방 프리팹
    public GameObject chestRoomPrefab;        // 상자방 프리팹
    public GameObject restRoomPrefab;         // 휴식방 프리팹
    public GameObject blockedRoomPrefab;      // 막힌 방 프리팹
    public GameObject bossRoomPrefab;         // 보스 방 프리팹


    Vector2Int player_Pos;             // 플레이어 시작 위치
    int gridSize = 4; // 그리드 크기
    int roomSpacing = 25;              // 방 간격
    // 최소/최대 방 개수
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
    [SerializeField] List<GameObject> map_Icon; // 0 : 위, 1 : 아래, 2 : 왼쪽, 3 : 오른쪽


    private List<Vector2Int> random_Start_Pos = new List<Vector2Int>
    {
        new Vector2Int(0, 0),   // 좌상단
        new Vector2Int(0, 3),   // 좌하단
        new Vector2Int(3, 0),   // 우상단
        new Vector2Int(3, 3),   // 우하단
    };

    Vector2Int[] directions = new Vector2Int[] // 방향 배열
    {
            new Vector2Int(0, -1),  // 위
            new Vector2Int(0, 1), // 아래
            new Vector2Int(-1, 0), // 왼쪽
            new Vector2Int(1, 0)   // 오른쪽
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
            BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start); // 첫방 시작
        }
        else
        {
            StartCoroutine(GenerateDungeon());
        }    
    }

    // 난이도에 따른 수치 변경
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

                nav_Surface.BuildNavMeshAsync();
                SetRoom(player_Pos);
                MiniMapUpdate(player_Pos);

                BattleManager.Instance.ChangePhase(BattleManager.BattlePhase.Start); // 첫방 시작

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
                Row row = new Row();
                for (int x = 0; x < gridSize; x++)
                {
                    Cell cell = new Cell();
                    row.cells.Add(cell);
                }
                mapRows.Add(row);
            }
        }
        // 시작지점 미리 할당
        Vector2Int startPos = random_Start_Pos[Random.Range(0, random_Start_Pos.Count)];
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
        Cell bossRoomCell = mapRows[bossRoomPos.y].cells[bossRoomPos.x];

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



    // 던전을 초기화하는 함수 (기존 방 삭제)
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

        if (randomValue < 85)  // 85% 확률로 전투 방 배치
        {
            return battleRoomPrefabs[Random.Range(0, battleRoomPrefabs.Length)];
        }
        else if (randomValue < 90)  // 5% 확률로 상자 방
        {
            return chestRoomPrefab;
        }
        else  // 10% 확률로 휴식 방
        {
            return restRoomPrefab;
        }
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
        if (mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj != null)
        {
            mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.SetActive(true);
            mapRows[cur_Room.y].cells[cur_Room.x].isVisit = true; // 현재방 방문
            mapRows[cur_Room.y].cells[cur_Room.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.white; // 현재방 색깔은 하얀색으로 변경

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
                    mapRows[newPos.y].cells[newPos.x].minimap_Obj.SetActive(true); // 상하좌우로 인접한 방
                    mapRows[newPos.y].cells[newPos.x].minimap_Obj.GetComponent<SpriteRenderer>().color = Color.gray; // 상하좌우로 인접한 방은 회색으로 변경
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
                if (IsValid(neighborPos) && !IsBlock(neighborPos) && !connectedRooms.Contains(neighborPos))
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
        foreach (Row row in mapRows)
        {
            foreach (Cell cell in row.cells)
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

    // Cell 객체의 좌표를 찾아 반환하는 함수
    private Vector2Int GetCellPosition(Cell cell)
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

    // 팝업에서 확인 버튼 누를 때 사용하는 룸 이동 메서드
    public void ConfirmRoom()
    {
        MoveToRoom(select_Direction);
    }

    // 실제로 방 생성 전 정보를 임시로 할당
    private void SetRoomValue(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        // 현재 셀에 복제된 오브젝트를 저장
        Cell cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomPrefab;
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
    }

    // 현재 할당된 정보를 바탕으로 방 생성
    private void PlaceAllRooms()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Cell cell = mapRows[y].cells[x];

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
        Cell cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomInstance;  // 복제된 인스턴스를 저장
        cell.isBlocked = isBlocked;
        cell.isBoss = isBoss;
        cell.minimap_Obj = PlaceMinimap(map_Tf, map_Mark_Prefab, pos, isBlocked);
        cell.isVisit = isVisit;
    }

    // 던전에 맞춰서 미니맵 생성 메서드
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

    // 방 이동 시 방 카메라와 미니맵 카메라 이동 메서드
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



