using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Cell2
{
    public GameObject cellObject;  // 방 오브젝트 (프리팹)
    public bool isClear;           // 방 클리어 여부
    public bool isBlocked;         // 방이 막혀 있는지 여부 (막힌 방일 경우 true)
    public bool isBoss;            // 보스방 여부
}

[System.Serializable]
public class Row2
{
    public List<Cell2> cells = new List<Cell2>();  // 한 행의 셀 리스트
}



public class TestMap : MonoBehaviour
{
    public List<Row2> mapRows = new List<Row2>();  // 4x4 그리드의 방 배열
    public GameObject startPrefab;
    public GameObject[] battleRoomPrefabs;       // 전투방 프리팹들
    public GameObject chestRoomPrefab;           // 상자방 프리팹
    public GameObject restRoomPrefab;            // 휴식방 프리팹
    public GameObject blockedRoomPrefab;         // 막힌 방 프리팹
    public GameObject bossRoomPrefab;            // 보스방 프리팹
    public Vector2Int player_Pos; // 플레이어 시작 위치
    public int gridSize = 4;                     // 4x4 그리드 크기
    public int roomSpacing = 25;                 // 방 간격

    // 최소 방 갯수
    public int maxBlockedRooms = 4;              // 최대 막힌 방 갯수
    public int minChestRooms = 1;                // 최대 상자 방 갯수
    public int minRestRooms = 1;
    public int minBattleRooms = 3;

    private List<Vector2Int> availableBossPositions = new List<Vector2Int>
    {
        new Vector2Int(0, 0), // 왼쪽 위
        new Vector2Int(0, 3),    // 왼쪽 아래
        new Vector2Int(3, 3),   // 오른쪽 아래
        new Vector2Int(3, 0),   // 오른쪽 위
        
    };

    Vector2Int[] directions = new Vector2Int[] // 방향 배열
    {
            new Vector2Int(0, -1),  // 위
            new Vector2Int(0, 1), // 아래
            new Vector2Int(-1, 0), // 왼쪽
            new Vector2Int(1, 0)   // 오른쪽
    };


    private void Start()
    {
        // 던전 생성
        GenerateMap();
    }

    // 던전 레이아웃을 생성하는 함수
    private void GenerateMap()
    {
        // 그리드 초기화 (4x4 셀 생성)
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

        HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>(); // 이미 배치된 방을 추적

        // 보스방 위치를 무작위로 선택
        Vector2Int startPos = StartEndPos(out Vector2Int bossPos);
        player_Pos = startPos;

        PlaceRoom(startPos, startPrefab);

        
        occupiedPositions.Add(startPos); // 시작 방은 이미 배치된 것으로 간주

        // 시작 방에서 보스방으로 가는 경로를 설정 (미로처럼)
        List<Vector2Int> pathToBoss = GenerateComplexPath(player_Pos, bossPos);

        // 경로에 있는 방들을 배치 (랜덤하게)
        foreach (var pathPos in pathToBoss)
        {
            if (pathPos == bossPos)
            {
                // 보스방 배치
                PlaceRoom(pathPos, bossRoomPrefab, isBoss: true);
                occupiedPositions.Add(pathPos);
            }
            else if (pathPos != startPos && !occupiedPositions.Contains(pathPos))  // 시작 방에는 이미 방이 배치되어 있으므로 제외
            {
                // 경로 위에도 무작위 방 배치
                GameObject randomRoom = GetRandomRoomPrefab();
                PlaceRoom(pathPos, randomRoom);
                occupiedPositions.Add(pathPos);
            }
        }

        // 경로를 제외한 방 생성
        PlaceMandatoryRooms(pathToBoss, bossPos);

    }

    // 시작방과 보스방 정하는 메서드
    private Vector2Int StartEndPos(out Vector2Int bossPos)
    {
        // 시작 방 위치를 결정
        int startRoomIndex = Random.Range(0, availableBossPositions.Count);
        Vector2Int startPos = availableBossPositions[startRoomIndex];

        // 가장 먼 위치에 있는 보스방 좌표를 계산
        bossPos = GetFurthestCorner(startPos);

        return startPos;
    }

    // 주어진 시작 방에서 가장 먼 코너 방을 반환하는 함수 (맨해튼 거리 계산)
    private Vector2Int GetFurthestCorner(Vector2Int startPos)
    {
        Vector2Int furthestPos = Vector2Int.zero;  // 기본값
        int maxDistance = -1;  // 현재까지의 최대 거리

        // 모든 코너 방 중에서 가장 먼 좌표 찾기
        foreach (Vector2Int cornerPos in availableBossPositions)
        {
            int distance = Mathf.Abs(startPos.x - cornerPos.x) + Mathf.Abs(startPos.y - cornerPos.y); // 맨해튼 거리 계산
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPos = cornerPos;
            }
        }

        return furthestPos;
    }

    // 시작 방에서 보스방까지 가는 복잡한 경로를 생성하는 함수
    private List<Vector2Int> GenerateComplexPath(Vector2Int startPos, Vector2Int endPos)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentPos = startPos;
        path.Add(currentPos);

        while (currentPos != endPos)
        {
            List<Vector2Int> possibleDirections = new List<Vector2Int>();

            // 수평 이동이 필요하다면 수평 방향 추가
            if (currentPos.x != endPos.x)
            {
                if (currentPos.x < endPos.x)
                    possibleDirections.Add(Vector2Int.right);
                else
                    possibleDirections.Add(Vector2Int.left);
            }

            // 수직 이동이 필요하다면 수직 방향 추가
            if (currentPos.y != endPos.y)
            {
                if (currentPos.y < endPos.y)
                    possibleDirections.Add(Vector2Int.up);
                else
                    possibleDirections.Add(Vector2Int.down);
            }

            // 무작위로 한 번 더 돌아가는 경로 추가 (경로를 꼬는 역할)
            if (Random.Range(0, 10) > 7) // 30% 확률로 경로를 꼼
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

            // 무작위로 방향 선택
            Vector2Int randomDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

            // 선택한 방향으로 이동
            currentPos += randomDirection;
            path.Add(currentPos);
        }

        return path;
    }

    // 최소 방 갯수를 충족하는 방들을 배치하는 함수
    private void PlaceMandatoryRooms(List<Vector2Int> pathToBoss, Vector2Int bossPos)
    {
        List<Vector2Int> availablePositions = GetAvailablePositions(pathToBoss, bossPos);

        // 막힌 방 배치
        for (int i = 0; i < maxBlockedRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            PlaceRoom(randomPos, blockedRoomPrefab, isBlocked: true);
        }

        // 상자 방 배치
        for (int i = 0; i < minChestRooms; i++)
        {
            Vector2Int randomPos = GetRandomPosition(availablePositions);
            PlaceRoom(randomPos, chestRoomPrefab);
        }

        // 휴식 방
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


    // 경로와 보스방이 차지하지 않는 빈 자리를 반환하는 함수
    private List<Vector2Int> GetAvailablePositions(List<Vector2Int> pathToBoss, Vector2Int bossPos)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                // 보스방과 경로는 제외
                if (pos != bossPos && !pathToBoss.Contains(pos) && pos != player_Pos)
                {
                    availablePositions.Add(pos);
                }
            }
        }

        return availablePositions;
    }

    // 사용 가능한 위치 중 하나를 무작위로 선택하고 해당 위치를 제거하는 함수
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

    // 방을 특정 위치에 배치하고 막힌 방 플래그 설정하는 함수
    private void PlaceRoom(Vector2Int pos, GameObject roomPrefab, bool isBlocked = false, bool isBoss = false)
    {
        Cell2 cell = mapRows[pos.y].cells[pos.x];
        cell.cellObject = roomPrefab;
        cell.isBoss = isBoss;
        cell.isBlocked = isBlocked;  // 막힌 방이면 true, 아니면 false

        // 월드에 방을 생성 (위치 계산)
        Vector3 roomPos = new Vector3(pos.x * roomSpacing, pos.y * -roomSpacing, 0);
        Instantiate(roomPrefab, roomPos, Quaternion.identity);
    }
}
