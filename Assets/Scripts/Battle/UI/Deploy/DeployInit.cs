using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeployInit : MonoBehaviour
{
    private Tilemap tilemap; // 타일맵
    public GameObject spritePrefab; // 배치할 스프라이트 프리팹
    public GameObject deloy_obj; // 클론이 생성 될 빈 개체

    public GameObject buff_Prefab;
    public Transform buff_Deploy_Tf;

    private int butt_Count = 4;

    public List<GameObject> highlight = new List<GameObject>();
    List<GameObject> buff = new List<GameObject>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        PlaceSpritesInTilemap();
       
    }

    private void PlaceSpritesInTilemap()
    {
        // 타일맵의 셀 크기 가져오기
        Vector3 cellSize = tilemap.cellSize;

        // 타일맵의 모든 셀 반복
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                // 현재 셀에 타일이 있는지 확인
                if (tilemap.HasTile(cellPos))
                {
                    // 현재 셀의 월드 좌표 계산
                    Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cellPos);

                    // 스프라이트를 셀의 중심에 배치
                    GameObject sprite = Instantiate(spritePrefab, cellWorldPos, Quaternion.identity);

                    // 배치된 스프라이트의 크기를 셀의 크기에 맞게 조정
                    sprite.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1f);

                    sprite.transform.SetParent(deloy_obj.transform);
                    highlight.Add(sprite);
                }
            }
        }

        foreach (GameObject high in highlight)
        {
            if (high != null && high.activeSelf) 
                high.SetActive(false);
        }
    }

    private void Update()
    {
        if (BattleManager.Instance.buff_On)
        {
            SelectBuffTile();
            BattleManager.Instance.buff_On = false;
        }

        if (BattleManager.Instance._curphase == BattleManager.BattlePhase.Battle)
        {
            if (buff_Deploy_Tf.gameObject.activeSelf)
                buff_Deploy_Tf.gameObject.SetActive(false);
        }
    }


    private void SelectBuffTile()
    {
        List<GameObject> availableTiles = new List<GameObject>(highlight); // 선택 가능한 타일 복사본

        for (int i = 0; i < Random.Range(1, butt_Count); i++)
        {
            if (availableTiles.Count == 0)
            {
                Debug.LogWarning("선택 가능한 타일이 부족합니다.");
                break; // 선택 가능한 타일이 없으면 종료
            }

            // 랜덤으로 타일 선택
            int randomIndex = Random.Range(0, availableTiles.Count);
            GameObject selectedTile = availableTiles[randomIndex];

            // 선택된 타일을 버프 목록에 추가하고 복사본에서 제거
            buff.Add(selectedTile);
            availableTiles.RemoveAt(randomIndex);

            int randomBuff = Random.Range(0, butt_Count);

            // 버프 생성
            CreateBuff(selectedTile, randomBuff);
        }
    }

    private void CreateBuff(GameObject buffTile, int buff_Index)
    {
        Vector3 pos = buffTile.transform.position;
        GameObject buff_Obj = Instantiate(buff_Prefab, pos, Quaternion.identity, buff_Deploy_Tf);

        buff_Obj.GetComponent<BuffInit>().Init(buff_Index);

    }
}
